using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMap
{

    private static EntityMap instance;

    public static EntityMap getInstance()
    {
        if (instance == null)
            instance = new EntityMapImp();
        return instance;
    }

    public abstract void Initialized();
    public abstract void generateEntityMap();
    public abstract Dictionary<int, Object> getEntityMap();
}

