using System;
using System.Net;
using System.Text;
using UnityEngine;

public class ConnectionImp : Connection {

    ConnectionProperties cp;
    private byte[] data;

    public override void Initialized() {
        cp = new ConnectionProperties();
        data = new byte[1024];
    }

    public override void sendEvent(bool send, object ev) {
        String info = "";
        if (ev.GetType() == typeof(System.String)) {
            info = (String)ev;
        } else if (ev.GetType() == typeof(GameEvent)) {
            //showGameEventStructure(ev);
            info = ((GameEvent)ev).toJSONObject().ToString();
        }

        if (send) {
            data = Encoding.ASCII.GetBytes(info);
            cp.getSocketClient().Send(data, data.Length);
        } else { Debug.Log(info); }
    }

    public override GameEvent ReceivedEvent() {
        IPEndPoint sender = cp.getSender();
        GameEvent ge = ScriptableObject.CreateInstance<GameEvent>();
        #pragma warning disable 0168
        try {
            data = cp.getSocketServer().Receive(ref sender);
            string dataSocket = Encoding.ASCII.GetString(data, 0, data.Length);
            ge.fromJSONObject(new JSONObject(dataSocket));
        }
        catch ( Exception e ) { /*Debug.Log(e.Message);*/ }
        GameEvent p = parseEvent(ge);
        //if (p.Name == "action") { Debug.Log(p.toJSONObject().ToString()); }
        return p;
    }

    /* --- HERRAMIENTAS --- */

    private void showGameEventStructure(GameEvent ge) {
        Debug.Log(ge.toJSONObject().ToString());
        String GameEvent = ge.name + "(";

        foreach (String p in ge.Params) {
            GameEvent += p + ", ";
        }
        GameEvent = GameEvent.Substring(0, GameEvent.Length - 2);
        GameEvent += ")";

        GameEvent += ")";
        Debug.Log(GameEvent);
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
                }/* else {
                    Debug.Log("====================================");
                    Debug.Log("tipo: " + param.GetType() + ", valor: " + param);
                    Debug.Log("====================================");
                }*/
            }
        }

        return ge;
    }
}