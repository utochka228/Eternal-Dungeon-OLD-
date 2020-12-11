using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

interface IItem
{
    void Use(Transform user);
}

public abstract class Item : ScriptableObject, IItem
{
    public new string name;
    public GameObject itemEffect;
    public float spawnChance;
    public Sprite sprite;
    public Vector3 spawnOffset;
    public float spawnRotation = 0f;
    public bool isStackable;

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
    public ItemActions actions;
    public void Use(Transform user)
    {
        UseItem(user);
    }

    public abstract void UseItem(Transform user);
}

[System.Serializable]
public class ItemActions {
    public bool isUsable;
    public bool isEquipable;

}