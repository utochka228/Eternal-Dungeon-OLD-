using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "Block", menuName = "BlockMenu")]
public class BlockBase : ScriptableObject
{
    [SerializeField] SpriteAtlas atlas;
    [Range(0f, 1f)] public float spawnChance;
    public Drop drop;
    [SerializeField] string blockSprite;
    [HideInInspector] public float health;
    [SerializeField] int hitCount = 1;
    public string BlockName{
        get{ return this.name;}
    }
    public float minPickaxePower = 10f;

    public Sprite BlockSprite{
        get { return atlas.GetSprite(blockSprite);}
    }

    private void OnEnable() {
        health = minPickaxePower * hitCount;
    }

}
[System.Serializable]
public struct Drop{
    [SerializeField] DropElement[] dropList;
    public (Item, Vector2Int) this[int index]
    {
        get { 
            if(index < 0 || index >= dropList.Length)
                return (null, Vector2Int.zero);
            return (dropList[index].drop, dropList[index].count);
        }
    }

    public int Length {
        get { return dropList.Length;}
    }
}
[System.Serializable]
public struct DropElement{
    public Item drop;
    public Vector2Int count;
}