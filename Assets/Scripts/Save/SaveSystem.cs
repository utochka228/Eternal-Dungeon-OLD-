using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    Saves sv = new Saves();

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        LoadSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(){
        #if UNITY_ANDROID
 
        #endif
        #if UNITY_EDITOR

        #endif
    }
    public void LoadSave(){
        if(!PlayerPrefs.HasKey("Save")){
            //
        } else{
            sv = JsonUtility.FromJson<Saves>(PlayerPrefs.GetString("Save"));
            //
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
    //level seeds
    //levels body
    //public int LastDungeonLevel
    //public int lastCheckPoint
    //public int CurrentDungeonLevel
}
[System.Serializable]
public class PlayerSaves{
    //inventory
    //last closest save-zone where was player
    //player statistic
    //character stats
    //death point, death loot, death level
}