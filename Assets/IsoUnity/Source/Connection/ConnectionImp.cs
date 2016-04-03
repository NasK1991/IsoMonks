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

    public override GameEvent getResultEvent(GameEvent ge) {
        GameEvent empty = ScriptableObject.CreateInstance<GameEvent>();
        sendEvent(ge);
        GameEvent resultGe = ReceivedEvent();               
        //Debug.Log(ge.toJSONObject().ToString());
        //Debug.Log(resultGe.toJSONObject().ToString());
        return (resultGe == empty) ? ge : resultGe;
    }

    private void sendEvent(GameEvent ge) {
        data = Encoding.ASCII.GetBytes(ge.toJSONObject().ToString());
        cp.getSocketClient().Send(data, data.Length);
    }

    private GameEvent ReceivedEvent() {
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
}