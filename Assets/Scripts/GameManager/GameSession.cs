using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameSession : MonoBehaviour
{
    public static GameSession instance;
    public Action PlayerDied;

    [SerializeField]
    private GameObject playerPrefab;
    public GameObject damagePopupPrefab;
    public Transition transition;
    public PlayerController Player { get; private set; }
    [SerializeField] CinemachineVirtualCamera virtualCamera;
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
    }

    public static void SpawnPlayer(Vector3 position)
    {
        GameSession session = instance;
        if(session.Player != null){
            session.Player.transform.position = position;
            return;
        }
        GameObject player = Instantiate(session.playerPrefab);
        session.virtualCamera.Follow = player.transform;
        player.transform.position = position;
        session.Player = player.GetComponent<PlayerController>();
        //Upload player skin look data
        var playerLook = Resources.Load<SkinPreset>("Skin/PlayerLook");
        session.Player.skinPartsHolder.UpdateSkinLook(playerLook);
        GameObject[] playerTarget = new GameObject[1];
        playerTarget[0] = player;
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
