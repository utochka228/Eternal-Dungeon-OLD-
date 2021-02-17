using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    public Enemy myEnemy;
    public override int Health
    {
        get{return health;}
            
        set
        {
            if (value >= 0)
            {
                health = value;
            }
        }
    }
    void Start()
    {
        myEnemy = GetComponent<Enemy>();
    }

    protected override void Dying(GameObject murderer)
    {
        DropLoot();
        Destroy(myEnemy.gameObject);
    }

    void DropLoot()
    {
        Debug.Log("Loot was dropped!");
    }

    protected override void TakingDamage(GameObject hitter, int damage, bool isCritical)
    {
        health -= damage;
        Vector3 center = GetComponent<Collider2D>().bounds.center;
        DamagePopup.Create(center, damage, isCritical);
        if (health == 0)
            Die(hitter);
    }

}
