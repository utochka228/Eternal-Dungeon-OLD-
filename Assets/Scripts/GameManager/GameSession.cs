using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession instance;
    public Action PlayerDied;

    [SerializeField]
    private GameObject playerPrefab;
    public GameObject damagePopupPrefab;
    public Transition transition;
    public PlayerController Player { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public void StartSession(){
        if(PlayerPrefs.HasKey("Save")){
            GameMap.GM.relocation.CreateSavedLevel();
            return;
        }else
            GameMap.GM.relocation.ChangeLevel();
        transition.Create(GameMap.GM.relocation.CurrentDungeonLevel, "sfsfssf");
    }

    public void SpawnPlayer(Vector3 position)
    {
        if(Player != null){
            Player.transform.position = position;
            return;
        }

        GameObject player = Instantiate(playerPrefab);
        player.transform.position = position;
        Player = player.GetComponent<PlayerController>();

        GameObject[] playerTarget = new GameObject[1];
        playerTarget[0] = player;
        CameraMultiTarget.instance.SetTargets(playerTarget);
    }
    public void DestroyPlayer(){
        Destroy(Player.gameObject);
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
