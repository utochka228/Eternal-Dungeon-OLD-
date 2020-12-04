using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : GameTypeBase
{
    [SerializeField]
    private float timeToSurvive = 60f;

    [SerializeField]
    private float spawningItemsDelay = 3f;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject[] itemsArray;

    private float timer;

    [SerializeField] GameObject exitForPlayer;

    new void Start()
    {
        base.Start();
        GenerateGameMap(mapSize);
        GameMap.GM.MapSize = mapSize;
        timer = timeToSurvive;
        if (itemsArray.Length > 0)
            StartCoroutine(SpawnItemsWithDelay(itemsArray, spawningItemsDelay));
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
        if(matchStarted)
            CountDown();

        if (Input.GetKeyDown(KeyCode.Tab))
            SpawnEnemy(enemyPrefab);
    }

    void CountDown()
    {
        timer -= Time.unscaledDeltaTime;

        if(timer <= 0f)
        {
            playerWon = true;
            MenuManager.MG.EndMatch();
        }
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
