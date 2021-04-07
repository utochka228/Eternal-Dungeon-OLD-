using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LevelSeed{
    public int level;
    public string seed;
    public LevelSeed(int _level, string _seed)
    {
        level = _level;
        seed = _seed;
    }
}
public class Relocation : MonoBehaviour
{
    //Start level of dungeon
    public static int FirstDungeonLevel = -1;
    //End level of dungeon
    public static int LastDungeonLevel = -1;
    public static int LastOpenedLevel = -1;
    public static int CurrentDungeonLevel = -1;
    public static int checkPointDistance = 5; //Distance from one c.p to second
    public static int dungeonSize = 30;
    List<LevelSeed> levelSeeds = new List<LevelSeed>();

    private void Start() {
        SaveSystem.instance.OnSave += SaveData;
        GameActions.instance.MatchStarted += LoadRelocationData;
    }

    public static void InitFirstPlayData(){
        FirstDungeonLevel = 1;
        CurrentDungeonLevel = 0;
        LastOpenedLevel = 0;
        LastDungeonLevel = dungeonSize;
    }

    void SaveData(){
        MapSaves mapSaves = SaveSystem.instance.saves.mapSaves;
        mapSaves.LastOpenedLevel = LastOpenedLevel;
        mapSaves.LastDungeonLevel = LastDungeonLevel;
        mapSaves.FirstDungeonLevel = FirstDungeonLevel;
        mapSaves.seeds = new List<LevelSeed>(levelSeeds);
        mapSaves.CurrentDungeonLevel = GetClosestCheckPoint();
    }
    int GetClosestCheckPoint(){
    int distUp = 0;
    int distDown = 0;
        for (int i = CurrentDungeonLevel; i >= 1; i--)
        {
            if((i % checkPointDistance) == 0 || i == FirstDungeonLevel)
                break;
            distUp++;
        }
        for (int i = CurrentDungeonLevel; i < dungeonSize; i++)
        {
            if((i % checkPointDistance) == 0)
                break;
            distDown++;
        }
        if(distUp > distDown){
            int closest = CurrentDungeonLevel + distDown;
            if(closest > LastOpenedLevel)
                return CurrentDungeonLevel - distUp;
            else
                return CurrentDungeonLevel + distDown;
        }
        else 
            return CurrentDungeonLevel - distUp;
    }
    void LoadRelocationData(){
        if(!PlayerPrefs.HasKey("Save"))
            return;

        MapSaves mapSaves = SaveSystem.instance.saves.mapSaves;
        CurrentDungeonLevel = mapSaves.CurrentDungeonLevel;
        LastOpenedLevel = mapSaves.LastOpenedLevel;
        FirstDungeonLevel = mapSaves.FirstDungeonLevel;
        LastDungeonLevel = mapSaves.LastDungeonLevel;
        levelSeeds = new List<LevelSeed>(mapSaves.seeds);
    }
    RelocPanel lastRelocPanel;
    public void SetRelocationPanel(){
        Transform relocationPanel = PlayerUI.instance.relocationPanel.transform;
        Transform relocationHolder = PlayerUI.instance.relocationHolder.transform;
        Transform child = relocationHolder.GetChild(CurrentDungeonLevel-1);
        RelocPanel panel = child.GetComponent<RelocPanel>();
        lastRelocPanel?.UnMarkPlayer();
        panel.MarkPlayer();
        lastRelocPanel = panel;
        //Activate panel
        relocationPanel.gameObject.SetActive(true);
    }
    void SpawnRelocButton(int level){
        Transform relocationHolder = PlayerUI.instance.relocationHolder.transform;
        GameObject prefab = PlayerUI.instance.relocationButton;
        GameObject relocButton = Instantiate(prefab, relocationHolder);
        RelocPanel panel = relocButton.GetComponent<RelocPanel>();
        panel.SetData(level);
    }
    //Called after creating new level
    public void ChangeLevel(){
    //if is new level then create else relocate
    if(CurrentDungeonLevel >= LastOpenedLevel){
        CreateLevel(); //Creating new level
    }
    else{
        RelocateToNext(); //Relocation to next opened level
    }
    GameSession.instance.transition.Create(CurrentDungeonLevel, GameTips.GetTip());
    }
    public void LoadClosestSavedCheckPoint(){
        GameMap.GM.GenerateGameField(GameLevelType.CheckPoint);
        GameSession.instance.transition.Create(CurrentDungeonLevel, GameTips.GetTip());
    }
    void CreateLevel(){
        CurrentDungeonLevel++;
        LastOpenedLevel++;
        GameMap.DestroyGameField();
        string seed = "";
        if(IsBossLevel() == false){
            if((CurrentDungeonLevel % checkPointDistance) == 0 || CurrentDungeonLevel == 1)
                GameMap.GM.GenerateGameField(GameLevelType.CheckPoint);
            else
                seed = GameMap.GM.GenerateGameField(GameLevelType.SimpleLevel);
        } else
            GameMap.GM.GenerateGameField(GameLevelType.BossLevel);
        //Save this level seed (even "")
        levelSeeds.Add(new LevelSeed(CurrentDungeonLevel, seed));
        SpawnRelocButton(CurrentDungeonLevel);
    }
    void RelocateToNext(){
        CurrentDungeonLevel++;
        GameMap.DestroyGameField();
        LevelSeed lSeed = levelSeeds.Single(x => x.level == CurrentDungeonLevel);
        string seed = lSeed.seed;
        if(IsBossLevel() == false){
            if((CurrentDungeonLevel % checkPointDistance) == 0)
                GameMap.GM.GenerateGameField(GameLevelType.CheckPoint);
            else
                GameMap.GM.GenerateGameField(GameLevelType.SimpleLevel, seed);
        } else
            GameMap.GM.GenerateGameField(GameLevelType.BossLevel);
    }
    public void RelocateTo(int level){
        GameMap.DestroyGameField();
        LevelSeed lSeed = levelSeeds.Single(x => x.level == level);
        string seed = lSeed.seed;
        CurrentDungeonLevel = level;
        if((CurrentDungeonLevel % checkPointDistance) == 0)
            GameMap.GM.GenerateGameField(GameLevelType.CheckPoint);
        else
            GameMap.GM.GenerateGameField(GameLevelType.SimpleLevel, seed);
    }
    bool IsBossLevel(){
        if(CurrentDungeonLevel == LastDungeonLevel)
            return true;
        return false;
    }
    public void BossDefeated(){
        //Set firstdungeonlevel
        CurrentDungeonLevel++;
        FirstDungeonLevel = CurrentDungeonLevel;
        //Clear prev game saves
    }
}
public enum GameLevelType { CheckPoint, SimpleLevel, BossLevel}