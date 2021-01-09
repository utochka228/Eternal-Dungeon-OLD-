using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
[CreateAssetMenu(fileName = "NonFuncProp", menuName = "CreateProp/NonFuncProp")]
public class Prop : ScriptableObject {
    public string propName;
    float health;
    [SerializeField] SpriteAtlas atlas;
    [SerializeField] string spriteName;

    public bool destroyable;
    public Sprite sprite {
        get {return atlas.GetSprite(spriteName);}
    }
    public float Health {
        get {
            return health;
        }

        set {
            health = value;
        }
    }
    public virtual void UseProp() { Debug.Log($"This is {propName} prop, without functionality!");}

}
