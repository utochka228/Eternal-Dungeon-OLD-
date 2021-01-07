using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "Block", menuName = "BlockMenu")]
public class BlockBase : ScriptableObject
{
    [SerializeField] SpriteAtlas atlas;
    public float spawnChance;
    public Drop drop;
    [SerializeField] string blockSprite;
    public float Health = 3;
    public string BlockName{
        get{ return this.name;}
    }
    public float minPickaxePower = 10f;

    public Sprite BlockSprite{
        get { return atlas.GetSprite(blockSprite);}
    }

}
[System.Serializable]
public struct Drop{
    [SerializeField] Item[] dropList;
    [SerializeField] int[] maxCount;
    public (Item, int) this[int index]
    {
        get { 
            if(index < 0 || index >= dropList.Length)
                return (null, 0);
            return (dropList[index], maxCount[index]);
        }
    }

    public int Length {
        get { return dropList.Length;}
    }
}