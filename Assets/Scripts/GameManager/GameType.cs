using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


//Responsible for GameTypes settings, spawn
public class GameType : MonoBehaviour
{
    public static GameType instance; //Singleton
    #region PublicVars


    public Action MatchStarted;
    public Action MatchEnded;
    #endregion

    #region PrivateVariables
    GameTypes gameType;

    [SerializeField]
    private GameObject[] gameTypePrefabs;
    LoaderBase currentGameTypeLoader;
    IResultMatch resultatorMatch;

    
    [SerializeField] GameObject gameLoader;
    #endregion

    void Awake()
    {
        instance = this;
    }

    void Start(){
         //For start of match
        MatchStarted += RandomizeGameType;
        MatchStarted += SpawnGameLoader;
        MatchStarted += MenuPresenter.instance.HideOtherUI;

        //For end of match
        
        MatchEnded += ResultMatch;
        MatchEnded += GameMap.GM.DestroyGameField;
        MatchEnded += CameraMultiTarget.instance.NullTargets;
        MatchEnded += CameraMultiTarget.instance.SetOldDataToCamera;
        MatchEnded += MenuPresenter.instance.ActiveMenuPanel;
        MatchEnded += MenuPresenter.instance.HideOtherUI;
    }

    public void StartMatch()
    {
        if(MatchStarted != null)
        {
            MatchStarted();
        }
    }
    public void EndMatch()
    {
        if(MatchEnded != null)
        {
            MatchEnded();
        }
    }

    public LoaderBase GetGameTypeLoader()
    {
        return currentGameTypeLoader;
    }
    
     void SpawnGameLoader()
    {
        Instantiate(gameLoader);
    }

    public void ResultMatch()
    {
        resultatorMatch.ResultMatch();
    }

    public void RandomizeGameType()
    {
        int randomNumber = Random.Range(0, Enum.GetNames(typeof(GameTypes)).Length);
        //gameTypePrefabs[randomNumber] VMESTO gameTypePrefabs[0]
        GameObject gameTypeObj = Instantiate(gameTypePrefabs[0]);
        currentGameTypeLoader = gameTypeObj.GetComponent<GameTypeBase>().loader;
        resultatorMatch = gameTypeObj.GetComponent<IResultMatch>();

        //gameType = (GameType)randomNumber;
        gameType = GameTypes.Survival;

    }
}
enum GameTypes { 
    Survival, 
    ClearingEnemies, 
    Dodge, 
    Painting, 
    DoNotFall, 
    Gathering
    }