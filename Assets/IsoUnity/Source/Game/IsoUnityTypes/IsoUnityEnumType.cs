using UnityEngine;
using System.Collections;

public class IsoUnityEnumType : IsoUnityType {
    private System.Enum enumerated;

    public override bool canHandle(object o) {
        return o is System.Enum;
    }

    public override IsoUnityType clone() {
        return IsoUnityEnumType.CreateInstance<IsoUnityEnumType>();
    }

    public override object Value {
        get {
            return enumerated;
        }
        set {
            enumerated = value as System.Enum;
        }
    }

    public override JSONObject toJSONObject() {
        JSONObject o = JSONObject.CreateStringObject(enumerated.ToString());
        return o;
        //throw new System.NotImplementedException();
    }

    public override void fromJSONObject(JSONObject json) {
        //throw new System.NotImplementedException();
    }
}
