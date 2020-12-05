using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IRelocation{
    void RelocateToCheckPoint(int indexPoint, int energyForReloc);
}
public class Survival : GameTypeBase, IRelocation
{
    [SerializeField]
    private float spawningItemsDelay = 3f;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject[] itemsArray;

    [SerializeField] GameObject exitForPlayer;

    public int LastDungeonLevel {get; private set;}
    public int lastCheckPoint = -1;
    public int CurrentDungeonLevel = -1;

    [SerializeField] int checkPointDistance = 5; //Distance from one c.p to second

    List<CheckPoint> checkPoints = new List<CheckPoint>();

    Dictionary<int, string> levelSeeds = new Dictionary<int, string>();

    new void Start()
    {
        base.Start();
        CreateLevel();
    }

    //void AddMoneyToPlayer(GameObject murderer)
    //{
    //    PlayerController player = murderer.GetComponent<PlayerController>();
    //    if(player == GameManager.GM.players[0])
    //    {
    //        MenuManager.MG.money += 6;
    //    }
    //}

    public override void AddEnemyDeath(GameObject murderer)
    {
        base.AddEnemyDeath(murderer);
        if(countOfKilledEnemies >= enemiesCount)
        {
            SpawnExitForPlayer();
        }
    }

    void SpawnExitForPlayer()
    {
        //SpawnExit
        GameObject exit = Instantiate(exitForPlayer);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            CreateLevel();
        if(Input.GetKeyDown(KeyCode.R))
            SetRelocationPanel();
    }

    void SetRelocationPanel(){
        Transform relocationPanel = PlayerUI.instance.relocationPanel.transform;
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
                continue;
            }
            neccessertEnergyText.text = energyForRelocation.ToString();
            
            PlayerStats stats = Player.GetComponent<PlayerStats>();
            if(stats.RelocationEnergy < energyForRelocation){
                button.interactable = false;
            }
        }
    }

    struct CheckPoint{
        int level;

        public CheckPoint(int currentLevel, ref int lastCheckPoint, in List<CheckPoint> checkPoints)
        {
            Debug.Log("check level =" + currentLevel);
            level = currentLevel;
            lastCheckPoint = currentLevel;
            checkPoints.Add(this);
        }        
        

        public int GetCheckPointLevel() {return level;}
    }
    //Called after creating new level
    void CreateLevel(){
        
        CurrentDungeonLevel++;
        LastDungeonLevel++;
        GameMap.GM.DestroyGameField();
        string seed = GameMap.GM.GenerateGameField(mapSize);
        Debug.Log($"Level : {CurrentDungeonLevel}");

        if(CurrentDungeonLevel == 0 || CurrentDungeonLevel == lastCheckPoint + checkPointDistance){
            CheckPoint checkPoint = new CheckPoint(CurrentDungeonLevel, ref lastCheckPoint, checkPoints);
        }

        levelSeeds.Add(CurrentDungeonLevel, seed);
    }

    public void RelocateToCheckPoint(int indexPoint, int energyForReloc){
        Debug.Log($"indexPoint {indexPoint}");
        int neccesseryLevel = checkPoints[indexPoint].GetCheckPointLevel();
        
        GameMap.GM.DestroyGameField();
        string seed = levelSeeds[neccesseryLevel];
        Debug.Log(seed+ " -  SEED!!In level " + neccesseryLevel );
        GameMap.GM.GenerateGameField(mapSize, seed);
        Player.GetComponent<PlayerStats>().RelocationEnergy -= energyForReloc;
        CurrentDungeonLevel = neccesseryLevel;
    } 

    public override void SpawnEnemy(GameObject enemy)
    {
        base.SpawnEnemy(enemy);
        Transform enemyTransform = Instantiate(enemy).transform;
        Vector2 randPos = GameMap.GM.RandomizePosionOnMap();
        enemyTransform.position = new Vector3(randPos.x, randPos.y, 0f);
    }
    public override void SpawnEnemy(Vector2 position)
    {
        base.SpawnEnemy(position);
        Transform enemyTransform = Instantiate(enemyPrefab).transform;
        enemyTransform.position = new Vector3(position.x, position.y, 0f);
    }
    
    public override void Result()
    {
        moneyTrophy += countOfKilledEnemies;

        base.Result();
    }

    public override void OnMatchStarted()
    {
        Debug.Log("Match Started!");
    }
}
