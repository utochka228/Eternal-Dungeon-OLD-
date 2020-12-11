using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDamageble
{
    [SerializeField] BlockBase blockBase;
    BlockBase myBlock;
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
        myBlock.Health -= damage;
        if (myBlock.Health <= 0)
            Die(hitter);
    }

    void DropLoot()
    {
        if (myBlock.drop == null)
            return;

        //Drop
    }

    void Start()
    {

    }

    public void SetBlock(BlockBase blockBase)
    {
        this.blockBase = blockBase;
        myBlock = Instantiate(this.blockBase);
        spriteRend.sprite = myBlock.blockSprite;
    }
}
