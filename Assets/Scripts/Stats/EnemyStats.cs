using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    public Enemy myEnemy;
    public int myDamage;
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
    [SerializeField] EnemyLoot myDropLoot;
    GameObject itemHolder;
    new void Start()
    {
        base.Start();
        myEnemy = GetComponent<Enemy>();
        itemHolder = Resources.Load<GameObject>("Items/ItemHolder");
    }

    protected override void Dying(GameObject murderer)
    {
        DropLoot();
        Destroy(myEnemy.gameObject);
    }

    void DropLoot()
    {
        Debug.Log("Loot was dropped!");
        if(myDropLoot == null)
            return;
        foreach (var loot in myDropLoot.possibleDropLoot)
        {
            int count = Random.Range(0, 5);
            Vector3 myPos = transform.position;
            for (int i = 0; i < count; i++)
            {
                GameObject holder = Instantiate(itemHolder, myPos, Quaternion.identity);
                ItemHolder itHolder = holder.GetComponent<ItemHolder>();
                itHolder.SetItem(loot, false);
            }
        }
    }
    protected override void Healing(int value){
        base.Healing(value);
        Health += value;
    }
    protected override void TakingDamage(GameObject hitter, int damage, bool isCritical)
    {
        base.TakingDamage(hitter, damage, isCritical);
        health -= damage;     
        if (health <= 0)
            Die(hitter);
    }

}
