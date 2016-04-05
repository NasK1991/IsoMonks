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

    public override void sendEvent(object ev) {
        String info = "";
        if (ev.GetType() == typeof(System.String)) {
            info = (String)ev;
        } else if (ev.GetType() == typeof(GameEvent)) {
            //showGameEventStructure(ev);
            info = ((GameEvent)ev).toJSONObject().ToString();
        }

        data = Encoding.ASCII.GetBytes(info);
        cp.getSocketClient().Send(data, data.Length);
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
        return ge;
    }

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
}