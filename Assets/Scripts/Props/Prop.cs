using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NonFuncProp", menuName = "CreateProp/NonFuncProp")]
public class Prop : ScriptableObject {
    public string propName;
    float health;
    public float Health {
        get {
            return health;
        }

        set {
            health = value;
        }
    }
    PropHolder myPropHolder;

    public virtual void UseProp() { Debug.Log($"This is {propName} prop, without functionality!");}
    public virtual void SetPropHolder(PropHolder _myPropHolder) { myPropHolder = _myPropHolder; }

}
