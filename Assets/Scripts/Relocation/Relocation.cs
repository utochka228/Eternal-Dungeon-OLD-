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
public class Relocation : MonoBehaviour, IRelocation
{
    public int LastDungeonLevel = -1;
    public int lastCheckPoint = -1;
    public int CurrentDungeonLevel = -1;
    [SerializeField] int checkPointDistance = 5; //Distance from one c.p to second
    List<CheckPoint> checkPoints = new List<CheckPoint>();
    Dictionary<int, string> levelSeeds = new Dictionary<int, string>();
    public void SetRelocationPanel(){
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
                                                    Quaternion.identity, relocationPanel);
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

    struct CheckPoint{
        public int level {get; private set;}

        public CheckPoint(int currentLevel, ref int lastCheckPoint, in List<CheckPoint> checkPoints)
        {
            level = currentLevel;
            lastCheckPoint = currentLevel;
            checkPoints.Add(this);
            GameMap.GM.SpawnCheckPointProp();
        }

        public int GetCheckPointLevel() {return level;}
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
    void CreateLevel(){
        CurrentDungeonLevel++;
        LastDungeonLevel++;
        GameMap.GM.DestroyGameField();
        string seed = GameMap.GM.GenerateGameField();
        if(CurrentDungeonLevel == 0 || CurrentDungeonLevel == lastCheckPoint + checkPointDistance){
            CheckPoint checkPoint = new CheckPoint(CurrentDungeonLevel, ref lastCheckPoint, checkPoints);
        }

        levelSeeds.Add(CurrentDungeonLevel, seed);
    }
    void RelocateToNext(){
        CurrentDungeonLevel++;
        GameMap.GM.DestroyGameField();
        string seed = levelSeeds[CurrentDungeonLevel];
        GameMap.GM.GenerateGameField(seed);
        try{
            CheckPoint point = checkPoints.Single(x => x.level == CurrentDungeonLevel);
            GameMap.GM.SpawnCheckPointProp();
        }catch{
            //Do nothing
        }
    }

    public void RelocateToCheckPoint(int indexPoint, int energyForReloc){
        int neccesseryLevel = checkPoints[indexPoint].GetCheckPointLevel();
        
        GameMap.GM.DestroyGameField();
        string seed = levelSeeds[neccesseryLevel];
        GameMap.GM.GenerateGameField(seed);
        GameSession.instance.Player.GetComponent<PlayerStats>().RelocationEnergy -= energyForReloc;
        CurrentDungeonLevel = neccesseryLevel;

        Transform relocationPanel = PlayerUI.instance.relocationPanel.transform;
        int childCount = relocationPanel.childCount;
        //Clear panel
        for (int i = 0; i < childCount; i++)
        {
            Destroy(relocationPanel.GetChild(i).gameObject);
        }
       relocationPanel.gameObject.SetActive(false);
    } 
}
