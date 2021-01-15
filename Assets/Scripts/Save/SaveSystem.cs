using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public Saves saves = new Saves();

    public Action OnSave;

    private void Awake() {
        instance = this;
        LoadSave();

    }

    void Start()
    {
    }

    [ContextMenu("DeletePrefs")]
    void DeletePrefs(){
        PlayerPrefs.DeleteKey("Save");
    }
    [ContextMenu("Save")]
    public void Save(){
        OnSave?.Invoke();
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(saves));
    }
    [ContextMenu("CheckNull")]
    void CheckNull(){
        bool b = saves.mapSaves.checkPoints == null;
        bool a = saves.mapSaves.seeds == null;
        Debug.Log("Null " + b);
        Debug.Log("Null seed" + a);
    }
    public void LoadSave(){
        if(!PlayerPrefs.HasKey("Save")){
            Debug.Log("SAVE is deleted");
        } else{
            Debug.Log("SAVE is here");
            saves = JsonUtility.FromJson<Saves>(PlayerPrefs.GetString("Save"));
        }
    }
    private void OnApplicationQuit() {
        Save();
    }
}

[System.Serializable]
public class Saves{
    public MapSaves mapSaves;
    public PlayerSaves playerSaves;
}
[System.Serializable]
public class MapSaves{
    //levels body
    public List<CheckPoint> checkPoints;
    public List<LevelSeed> seeds;
    public int LastDungeonLevel;
    public int lastCheckPoint;
    public int CurrentDungeonLevel;

    
}
[System.Serializable]
public class PlayerSaves{
    //inventory
    public int inventorySize;
    public List<SlotDataSave> inventory;
    //player statistic
    //character stats
    //death point, death loot, death level
    public DeathPointData deathPointData;
}