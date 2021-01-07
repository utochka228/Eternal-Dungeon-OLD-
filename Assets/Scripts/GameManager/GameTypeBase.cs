using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IResultMatch
{
    void ResultMatch();
}

public abstract class GameTypeBase : MonoBehaviour, IResultMatch
{
    public static GameTypeBase instance;
    public LoaderBase loader;
    public string gameTypeName = "DefaultName";
    public int moneyTrophy;
    public float expirience = 5f;

    public int LastDungeonLevel = -1;
    public int lastCheckPoint = -1;
    public int CurrentDungeonLevel = -1;

    public delegate void GameKillerHandler(GameObject killer);
    public event GameKillerHandler EnemyWasKilled;
    Action PlayerDied;


    [SerializeField]
    protected bool playerWon;

    protected bool matchStarted;

    protected int enemiesCount;
    protected int countOfKilledEnemies;

    [SerializeField]
    protected Vector2 mapSize;

    [SerializeField]
    private GameObject playerPrefab;
    public PlayerController Player { get; private set; }

    public abstract void OnMatchStarted();

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        PlayerDied += PlayerLostMatch;
        EnemyWasKilled += AddEnemyDeath;
        SpawnPlayer();
    }
    
    protected virtual void PlayerLostMatch()
    {
        PlayerDied -= PlayerLostMatch;
        GameType.instance.EndMatch();
    }

    public void ResultMatch()
    {
        Result();
    }

    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(3, 9, 0);
        Player = player.GetComponent<PlayerController>();

        GameObject[] playerTarget = new GameObject[1];
        playerTarget[0] = player;
        CameraMultiTarget.instance.SetTargets(playerTarget);
    }

    public virtual void Result()
    {
        Debug.Log("Resulting match of " + gameTypeName);

        if (playerWon)
        {
            Debug.Log("U Win Match!");
            PlayerInfo.PI.Money += moneyTrophy;
        }
        else
        {
            Debug.Log("U are lost match!");
        }

        PlayerInfo.PI.Expirience += expirience;
        Destroy(Player.gameObject);
        Destroy(gameObject);
    }

    public void CallEnemyKilledEvent(GameObject killer)
    {
        if(EnemyWasKilled != null)
        {
            EnemyWasKilled(killer);
        }
    }
    public void CallPlayerDieEvent()
    {
        if (PlayerDied != null)
        {
            PlayerDied();
        }
    }

    public virtual void AddEnemyDeath(GameObject murderer)
    {
        countOfKilledEnemies++;
    }
    public virtual void SpawnEnemy(GameObject enemy)
    {
        enemiesCount++;
    }
    public virtual void SpawnEnemy(Vector2 position)
    {
        enemiesCount++;
    }


    //Спавн вещей для взаимодействия
    public IEnumerator SpawnItemsWithDelay(GameObject[] spawingItemsList, float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            SpawnItemWithRandomPosition(spawingItemsList);

            yield return null;
        }
    }

    public void StopCoroutines()
    {
        Debug.Log("Stopping all coroutines");
        StopAllCoroutines();
    }

    private void SpawnItemWithRandomPosition(GameObject[] spawingItemsList)
    {
        Vector2 position = GameMap.GM.RandomizePosionOnMap();

        // GameObject go = Instantiate(spawingItemsList[ChooseItemInListOfItems(spawingItemsList)]);
        // go.transform.position = new Vector3(position.x, position.y, -go.GetComponent<Item>().spawningOffsetY);
        // go.transform.SetParent(transform);
    }

    int ChooseItemInListOfItems(GameObject[] items)
    {

        float total = 0;

        foreach (var elem in items)
        {
            total += elem.GetComponent<Item>().spawnChance;
        }

        float randomPoint = UnityEngine.Random.value * total;

        for (int i = 0; i < items.Length; i++)
        {
            if (randomPoint < items[i].GetComponent<Item>().spawnChance)
            {
                return (int)i;
            }
            else
            {
                randomPoint -= items[i].GetComponent<Item>().spawnChance;
            }
        }
        return items.Length - 1;
    }
}
