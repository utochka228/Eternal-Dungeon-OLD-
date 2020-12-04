using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDamageble
{
    public BlockBase blockBase;
    BlockBase myBlock;
    [SerializeField] SpriteRenderer spriteRend;
    public void Die(GameObject murderer)
    {
        DropLoot();
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
        SetBlock();
    }

    void SetBlock()
    {
        myBlock = Instantiate(blockBase);
        spriteRend.sprite = myBlock.blockSprite;
    }
}
