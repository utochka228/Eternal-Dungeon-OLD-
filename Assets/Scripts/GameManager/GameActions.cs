using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


//Responsible for GameTypes settings, spawn
public class GameActions : MonoBehaviour
{
    public static GameActions instance; //Singleton
    public Action MatchStarted;
    public Action MatchEnded;

    void Awake()
    {
        instance = this;
    }

    void Start(){
         //For start of match
        MatchStarted += MenuPresenter.instance.HideOtherUI;
        MatchStarted += GameSession.instance.StartSession;

        //For end of match
        MatchEnded += GameMap.GM.DestroyGameField;
        MatchEnded += GameSession.instance.DestroyPlayer;
        MatchEnded += MenuPresenter.instance.ActiveMenuPanel;
        MatchEnded += MenuPresenter.instance.HideOtherUI;
    }

    public void StartMatch()
    {
        if(MenuPresenter.playerInfo.newCharacter)
            MenuPresenter.playerInfo.newCharacter = false;
        MatchStarted?.Invoke();
    }
    public void EndMatch()
    {
        MatchEnded?.Invoke();
        SaveSystem.instance.Save();
    }
    
}