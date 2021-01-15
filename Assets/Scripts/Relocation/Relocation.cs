using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IRelocation{
    void SetRelocationPanel();
    void RelocateToCheckPoint(int indexPoint, int energyForReloc);
    void ChangeLevel();
}
[System.Serializable]
public struct CheckPoint{
        public int level {get; private set;}

        public CheckPoint(int currentLevel, ref int lastCheckPoint, in List<CheckPoint> checkPoints)
        {
            level = currentLevel;
            lastCheckPoint = currentLevel;
            checkPoints.Add(this);
        }

        public int GetCheckPointLevel() {return level;}
}
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
public class Relocation : MonoBehaviour, IRelocation
{
    public int LastDungeonLevel = -1;
    public int lastCheckPoint = -1;
    public int CurrentDungeonLevel = -1;
    [SerializeField] int checkPointDistance = 5; //Distance from one c.p to second
    List<CheckPoint> checkPoints = new List<CheckPoint>();
    List<LevelSeed> levelSeeds = new List<LevelSeed>();

    private void Start() {
        SaveSystem.instance.OnSave += SaveData;
        GameActions.instance.MatchStarted += LoadRelocationData;
    }

    void SaveData(){
        MapSaves mapSaves = SaveSystem.instance.saves.mapSaves;
        mapSaves.lastCheckPoint = lastCheckPoint;
        mapSaves.LastDungeonLevel = LastDungeonLevel;
        mapSaves.checkPoints = new List<CheckPoint>(checkPoints);
        mapSaves.seeds = new List<LevelSeed>(levelSeeds);
        for (int i = CurrentDungeonLevel; i >= 0; i--)
        {
            bool success = checkPoints.Any(x => x.level == i);
            if(success){
                mapSaves.CurrentDungeonLevel =  i;
                Debug.Log("ClosesSaveZone ==" + i);
                break;
            }
        }
    }

    bool IsCheckPointLevel(){
        bool success = checkPoints.Any(x => x.level == CurrentDungeonLevel);
        if(success){
            return true;
        }
        return false;
    }

    [ContextMenu("LoadRelocationData")]
    void LoadRelocationData(){
        if(!PlayerPrefs.HasKey("Save"))
            return;

        MapSaves mapSaves = SaveSystem.instance.saves.mapSaves;
        CurrentDungeonLevel = mapSaves.CurrentDungeonLevel;
        lastCheckPoint = mapSaves.lastCheckPoint;
        LastDungeonLevel = mapSaves.LastDungeonLevel;
        checkPoints = new List<CheckPoint>(mapSaves.checkPoints);
        levelSeeds = new List<LevelSeed>(mapSaves.seeds);
    }

    public void SetRelocationPanel(){
        Transform relocationHolder = PlayerUI.instance.relocationHolder.transform;
        Transform relocationPanel = PlayerUI.instance.relocationPanel.transform;
        //Activate panel
        relocationPanel.gameObject.SetActive(true);

        //Spawn buttons for checkpoints
        for(int i = 0; i < checkPoints.Count; i ++)
        {
            CheckPoint point = checkPoints[i];
            int pointLevel = point.GetCheckPointLevel();
            GameObject buttonPrefab = PlayerUI.instance.relocationButton;
            GameObject _button = Instantiate(buttonPrefab, buttonPrefab.transform.position, 
                                                    Quaternion.identity, relocationHolder);
            Button button = _button.GetComponent<Button>();
            int energyForRelocation = 0;
            if(CurrentDungeonLevel > pointLevel)
                energyForRelocation = 0;
            else 
                energyForRelocation = (point.GetCheckPointLevel() - CurrentDungeonLevel) * checkPointDistance;
            Debug.Log("Adding argument " + i + "to method");    

            RelocationButton rbutton = _button.GetComponent<RelocationButton>();
            rbutton.indexPoint = i;
            rbutton.relocationEnergy = energyForRelocation;

            TextMeshProUGUI neccessertEnergyText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if(pointLevel == CurrentDungeonLevel){
                neccessertEnergyText.text = "You here";
                button.interactable = false;
                continue;
            }
            neccessertEnergyText.text = energyForRelocation.ToString();
            
            PlayerStats stats = GameSession.instance.Player.GetComponent<PlayerStats>();
            if(stats.RelocationEnergy < energyForRelocation){
                button.interactable = false;
            }
        }
    }

    
    //Called after creating new level
    public void ChangeLevel(){
        
        //if is new level then create else relocate
        if(CurrentDungeonLevel >= LastDungeonLevel){
            CreateLevel();
        }
        else{
            RelocateToNext();
        }
        
    }
    public void CreateSavedLevel(){
        string seed = levelSeeds.Single(x => x.level == CurrentDungeonLevel).seed;
        GameMap.GM.GenerateGameField(IsCheckPointLevel(), seed);
        Debug.Log("CreatedSavedLevel!");
    }
    void CreateLevel(){
        CurrentDungeonLevel++;
        LastDungeonLevel++;
        GameMap.GM.DestroyGameField();
        if(CurrentDungeonLevel == 0 || CurrentDungeonLevel == lastCheckPoint + checkPointDistance){
            CheckPoint checkPoint = new CheckPoint(CurrentDungeonLevel, ref lastCheckPoint, checkPoints);
        }
        string seed = GameMap.GM.GenerateGameField(IsCheckPointLevel());
        levelSeeds.Add(new LevelSeed(CurrentDungeonLevel, seed));
        Debug.Log("Level Created!");
    }
    void RelocateToNext(){
        CurrentDungeonLevel++;
        GameMap.GM.DestroyGameField();
        LevelSeed lSeed = levelSeeds.Single(x => x.level == CurrentDungeonLevel);
        string seed = lSeed.seed;
        GameMap.GM.GenerateGameField(IsCheckPointLevel(), seed);
    }
    
    public void RelocateToCheckPoint(int indexPoint, int energyForReloc){
        int neccesseryLevel = checkPoints[indexPoint].GetCheckPointLevel();
        
        GameMap.GM.DestroyGameField();
        LevelSeed lSeed = levelSeeds.Single(x => x.level == neccesseryLevel);
        string seed = lSeed.seed;
        GameMap.GM.GenerateGameField(IsCheckPointLevel(), seed);
        GameSession.instance.Player.GetComponent<PlayerStats>().RelocationEnergy -= energyForReloc;
        CurrentDungeonLevel = neccesseryLevel;

        ClearRelocPanel();
        Transform relocationPanel = PlayerUI.instance.relocationPanel.transform;
        relocationPanel.gameObject.SetActive(false);

    } 

    public void ClearRelocPanel(){
        Transform relocationHolder = PlayerUI.instance.relocationHolder.transform;
        int childCount = relocationHolder.childCount;
        //Clear panel
        for (int i = 0; i < childCount; i++)
        {
            Destroy(relocationHolder.GetChild(i).gameObject);
        }
    }
}
