﻿public abstract class Connection {

    private static Connection instance;

    public static Connection getInstance() {
        if (instance == null)
            instance = new ConnectionImp();
        return instance;
    }

    public abstract void Initialized();
    public abstract GameEvent getResultEvent(GameEvent ge);
}
