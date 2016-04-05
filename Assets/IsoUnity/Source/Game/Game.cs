using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	Queue<GameEvent> events;
	
    /*
     *  Looking things (Recommended to use CameraManager.lookTo(<<Target>>) to manage camera).
     */
    public GameObject look; // Do not use this to know who's camera looking, WRITE ONLY (Use CameraManager instead)
    private GameObject previousLook; 

    /*
     * Initial Map of the game (Recommended to use MapManager to manage maps).
     */
	public Map map; // Do not use this to know what map is active, WRITE ONLY (Use MapManager instead)
    private Map previousMap;

    /*
     * Event Manager Things
     * Use this list to create the managers at the start of the game.
     * (By default Animation, Secuence and IsoSwitches Managers are created).
     */
	public List<string> managers = new List<string>(new string[]{"AnimationManager", "SecuenceManager", "IsoSwitchesEventManager"});
	private List<EventManager> eventManagers;


    /*
     * Screen controls default controller.
     */
	public bool onScreenControls;

    /*
     * Static main game instance
     */
	public static Game main;

    /*
     * Game initialization
     */
	void Start () {
		main  = this;

        // Event Queue
		events = new Queue<GameEvent>();


        // Main Managers initialization
        // TODO Make they event managers as the rest
        CameraManager.initialize();
		CameraManager.lookTo (look);
		MapManager.getInstance().hideAllMaps();
		MapManager.getInstance().setActiveMap(map);
		ControllerManager.Enabled = true;
		IsoSwitchesManager.getInstance().getIsoSwitches();
        Connection.getInstance().Initialized();

        //============================================
        EntityMap.getInstance().Initialized();
        EntityMap.getInstance().generateEntityMap(); //Se genera el Dictionary con <id,GameObject>
        //============================================

        // Event Managers Creation
        eventManagers = new List<EventManager> ();
		foreach(string manager in managers){
			eventManagers.Add (ScriptableObject.CreateInstance(manager) as EventManager);
		}

        // On Screen Controlls creation
        // TODO move this to GUI Manager as it becomes a regular EventManager
		if(this.onScreenControls)
			GUIManager.addGUI(new OnScreenControlsGUI(), 99);
	}
	
	void Update () {
		this.tick();
	}

	void OnGUI(){
		GUIManager.tick();
	}

    /*
     * Event methods
     */

	public void enqueueEvent(GameEvent ge){
		if(ge == null)
			return;

        Connection.getInstance().sendEvent(ge);
        ge = parseEvent(Connection.getInstance().ReceivedEvent());

        this.events.Enqueue(ge);
    }

    private GameEvent parseEvent(GameEvent ge) {
        foreach (string contenido_param in ge.Params) {
            object param = ge.getParameter(contenido_param);
            if (contenido_param.Equals("direction")) {
                Mover.Direction t = new Mover.Direction();
                switch ((System.String)param) {
                    case "North": ge.setParameter(contenido_param, t); break;
                    case "East": ge.setParameter(contenido_param, t + 1); break;
                    case "South": ge.setParameter(contenido_param, t + 2); break;
                    case "West": ge.setParameter(contenido_param, t + 3); break;
                }
            } else {
                if (param.GetType() == typeof(System.Int32)) {
                    int intParam = (int)param;
                    if (EntityMap.getInstance().getEntityMap().ContainsKey(intParam)) {
                        UnityEngine.Object go_src;
                        EntityMap.getInstance().getEntityMap().TryGetValue(intParam, out go_src);
                        ge.setParameter(contenido_param, go_src);
                    }
                } else {
                    Debug.Log("====================================");
                    Debug.Log("tipo: " + param.GetType() + ", valor: " + param);
                    Debug.Log("====================================");
                }
            }
        }
        
        return ge;
    }
    
	public void eventFinished(GameEvent ge){
		object sync = ge.getParameter("synchronous");
		if(sync!=null && ((bool)sync)){
			GameEvent f = ScriptableObject.CreateInstance<GameEvent>();
			f.Name = "Event Finished";
			f.setParameter("event", ge);
			this.enqueueEvent(f);
		}
	}

    // Private method used to broadcast the events in main tick
    private void broadcastEvent(GameEvent ge){
        foreach (EventManager manager in eventManagers)
            manager.ReceiveEvent(ge);

        foreach (Map map in MapManager.getInstance().getMapList())
            map.broadcastEvent(ge);
    }

    /*
     * As the player input isnt so frecuent, it's only checked each ms to improve performance
     */
    private float timeToController = 100 / 1000;
    private float currentTimeToController = 0;

	public void tick(){
        // Variable changes
        if (previousLook != look)
            CameraManager.lookTo(previousLook = look);

        if (previousMap != map)
            MapManager.getInstance().setActiveMap(previousMap = map);

        // Main Tick
		CameraManager.Update();

        // Controller management
		currentTimeToController+=Time.deltaTime;
		if(currentTimeToController > timeToController){
			ControllerManager.tick();
			currentTimeToController-=timeToController;
		}

        // Events launch
        while (events.Count > 0) {
            GameEvent ge = events.Dequeue();
			broadcastEvent(ge);
		}

        // EventManagers ticks
        foreach (EventManager manager in eventManagers) {
            manager.Tick();
        }

        // Entities ticks
		foreach(Map eachMap in MapManager.getInstance().getMapList()) {
            eachMap.tick();
		}
    }

}
