using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : Stats
{
    public PlayerController myController;
    HealthHearts myHearts;
    public override int Health
    {
        get
        {
            return health;
        }
        set
        {
            if (value >= 0)
            {
                if(value > maxHealth)
                    value = maxHealth;

                health = value;
                myHearts.UpdateHearts(this);
            }
        }
    }
    public override int MaxHealth{
        get {return maxHealth;}
        set{
            if(value >= 0){
                int oldMaxHp = maxHealth;
                bool changeHealth = false;
                if(Health == oldMaxHp)
                    changeHealth = true;
                maxHealth = value;
                if(maxHealth > oldMaxHp && changeHealth == true)
                    health = maxHealth;
                if(Health > maxHealth)
                    health = maxHealth;
                myHearts.UpdateHearts(this);
            }
        }
    }
    public int myEnergy;
    
    public int Energy { 
        get{
            return myEnergy;
        }
        set {
            if(value >= 0)
                myEnergy = value;
        } 
    }
    public int relocationEnergy;
    public int RelocationEnergy { 
        get{
            return relocationEnergy;
        }
        set {
            if(value >= 0)
                relocationEnergy = value;
        } 
    }
    public void Start()
    {
        //Other init
        myController = GetComponent<PlayerController>();
        myHearts = PlayerUI.instance.playerHearts;

        Health = MaxHealth;
    }
    protected override void Dying(GameObject murderer)
    {
        Debug.Log(myController.transform.name + " was killed by " + murderer.name);
        SaveDeathPoint();
        Inventory.instance.ClearInventory();
        GameSession.instance.PlayerDied?.Invoke();
    }

    protected override void TakingDamage(GameObject hitter, int damage)
    {
        health -= damage;
            if (health == 0)
                Die(hitter);
    }

    private void OnDestroy() {
        Inventory.instance.SaveInventory();
    }
    
    void SaveDeathPoint(){
        PlayerSaves playerSaves = SaveSystem.instance.saves.playerSaves;
        Relocation relocation = GameMap.GM.relocation;
        Vector2 deathPos = new Vector2(transform.position.x, transform.position.y);
        playerSaves.deathPointData = new DeathPointData(relocation.CurrentDungeonLevel, deathPos);
    }
}
[System.Serializable]
public struct DeathPointData{
    public bool createDeathPoint;
    public int levelOfDeath;
    public Vector2 position;
    public List<SlotDataSave> loot;

    public DeathPointData(int deathLevel, Vector2 pos){
        levelOfDeath = deathLevel;
        position = pos;
        loot = Inventory.instance.inventory.Select(x => x.slotDataSave).ToList();
        createDeathPoint = true;
    }
}