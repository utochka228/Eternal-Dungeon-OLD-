using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : Stats
{
    public PlayerController myController;
    UIPlayerUpdater playerUpdater;
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
                playerUpdater.healthText.text = health.ToString();
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
    void Start()
    {
        //Variables init
        health = PlayerInfo.PI.Health;


        //Other init
        myController = GetComponent<PlayerController>();
        playerUpdater = GetComponent<UIPlayerUpdater>();
    }
    protected override void Dying(GameObject murderer)
    {
        Debug.Log(myController.transform.name + " was killed by " + murderer.name);
        SaveDeathPoint();
        Inventory.instance.ClearInventory();
        GameSession.instance.PlayerDied?.Invoke();
    }

    protected override void TakingDamage(GameObject hitter)
    {
        health--;
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