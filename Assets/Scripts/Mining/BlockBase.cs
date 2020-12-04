using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "BlockMenu")]
public class BlockBase : ScriptableObject
{
    public float spawnChance;
    public GameObject drop;
    public Sprite blockSprite;
    public float Health = 3;
    public string blockName;
}
