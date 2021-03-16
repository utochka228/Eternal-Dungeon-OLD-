using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;

interface IUsable
{
    void Use(Transform user);
}
interface IEquipable{
    void Equip();
    void Unequip();
}
public abstract class Item : ScriptableObject, IUsable
{
    [SerializeField] SpriteAtlas atlas;
    public string itemName;
    public GameObject itemEffect;
    [Range(0f, 1f)] public float spawnChance;
    public int itemMinPrice;
    public float itemWorldScale = 1f;
    //[HideInInspector] public string spritePath;
    [SerializeField] string spriteName;
    public Sprite sprite {
        get { return atlas.GetSprite(spriteName);}
    }
    public Vector3 spawnOffset;
    public float spawnRotation = 0f;
    public bool isStackable;
    public int maxStack;

    int stackCount = 1;
    public int Count { 
        get{
            if(isStackable)
                return stackCount;
            else
                return 1;
        }
        set{
            if(value >= 1 && isStackable){
                stackCount = value;
            }
        }
    }
    public ItemActions[] actions;
    private void OnEnable() {
        //spritePath = AssetDatabase.GetAssetPath(sprite);
    }
    public void Use(Transform user)
    {
        UseItem(user);
    }

    public virtual void UseItem(Transform user){}
    public virtual string GetItemType(){return this.GetType().Name;}

}
[System.Serializable]
public class ItemActions {
    public ItemActionsEnum itemAcitons;
    
}
public enum ItemActionsEnum {Usable, Equipable}