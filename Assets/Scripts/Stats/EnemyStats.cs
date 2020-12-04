﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    public Enemy myEnemy;
    public override float Health
    {
        get
        {
            return health;
        }
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
        GameTypeBase.instance.CallEnemyKilledEvent(murderer);
        DropLoot();
        Destroy(myEnemy.gameObject);
    }

    void DropLoot()
    {
        Debug.Log("Loot was dropped!");
    }

    protected override void TakingDamage(GameObject hitter)
    {
        health--;
        if (health == 0)
            Die(hitter);
    }

}
