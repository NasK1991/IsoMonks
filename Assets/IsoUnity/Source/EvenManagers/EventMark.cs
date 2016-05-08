using UnityEngine;
using System.Collections;

public class EventMark : MonoBehaviour {

    public string sendEvent = "";

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public void responseEvent(string id) {
        switch(id) {
            case "suena_campana":
                this.GetComponentInChildren<AudioSource>().Play();
                break;
            default: Debug.Log("Id desconocido: " + id);  break;
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        string who = collisionInfo.gameObject.GetComponentInParent<Entity>().entityName;
        string json = "{\"name\":\"event\",\"parameters\":{\"cell\":" + this.GetInstanceID() + ",\"eventName\":\"" + this.sendEvent + "\",\"who\":\"" + who + "\"}}";
        Connection.getInstance().sendEvent(true, json);
        //Debug.Log("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
        //Debug.Log("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
        //Debug.Log("Their relative velocity is " + collisionInfo.relativeVelocity);
    }

    void OnCollisionStay(Collision collisionInfo) {
        //Debug.Log(gameObject.name + " and " + collisionInfo.collider.name + " are still colliding");
    }

    void OnCollisionExit(Collision collisionInfo) {
        //Debug.Log(gameObject.name + " and " + collisionInfo.collider.name + " are no longer colliding");
    }
}
