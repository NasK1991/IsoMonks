using UnityEngine;
using System.Collections;

public class EventMark : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    void OnCollisionEnter(Collision collisionInfo) {
        string json = "{\"name\":\"event\",\"parameters\":{\"eventName\":\"tocar_campana\"}}";
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
