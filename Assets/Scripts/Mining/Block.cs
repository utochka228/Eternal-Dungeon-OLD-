using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDamageble
{
    [SerializeField] BlockBase blockBase;
    BlockBase myBlock;
    [SerializeField] GameObject dropHolder;
    [SerializeField] SpriteRenderer spriteRend;
    public bool spawnExit;
    public void Die(GameObject murderer)
    {
        DropLoot();
        if(spawnExit)
            GameMap.GM.SpawnExit(transform.position);
        Destroy(gameObject);
    }

    public void TakeDamage(GameObject hitter, float damage)
    {
        if(damage < myBlock.minPickaxePower)
            return;

        Debug.Log("Damaged");
        myBlock.Health -= damage;
        if (myBlock.Health <= 0)
            Die(hitter);
    }

    void DropLoot()
    {
        Drop myDrop = myBlock.drop;
        if (myDrop.Length == 0)
            return;

        for (int i = 0; i < myDrop.Length; i++)
        {
            var drop = myDrop[i];
            int countOfDrop = Random.Range(0, drop.Item2+1);
            for (int y = 0; y < countOfDrop; y++)
            {
                GameObject dHolder = Instantiate(dropHolder, transform.position, Quaternion.identity) as GameObject;
                ItemHolder itemHolder = dHolder.GetComponent<ItemHolder>();
                itemHolder.SetItem(drop.Item1, true);
            }
        }
    }

    void Start()
    {

    }

    public void SetBlock(BlockBase blockBase)
    {
        this.blockBase = blockBase;
        myBlock = Instantiate(this.blockBase);
        spriteRend.sprite = myBlock.BlockSprite;
    }
}
