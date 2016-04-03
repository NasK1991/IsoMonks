using System.Collections.Generic;
using UnityEngine;

public class EntityMapImp : EntityMap{

    private Dictionary<int, Object> entityMap;    

    public override void Initialized(){
        entityMap = new Dictionary<int, Object> ();        
    }

    public override void generateEntityMap(){
        entityMap.Clear();

        foreach(Object go in Resources.FindObjectsOfTypeAll(typeof(Object))){
            entityMap.Add(go.GetInstanceID(), go);                        
        }        
    }

    public override Dictionary<int, Object> getEntityMap(){
        return entityMap;
    }
}
