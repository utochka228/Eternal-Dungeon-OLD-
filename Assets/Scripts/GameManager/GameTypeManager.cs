using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public enum GameType { Survival, ClearingEnemies, Dodge, Painting, DoNotFall, Gathering}

//Responsible for GameTypes settings, spawn
public class GameTypeManager : MonoBehaviour
{
    public static GameTypeManager GM; //Singleton
    #region PublicVars

    public GameType gameType;

    #endregion

    #region PrivateVariables

    [SerializeField]
    private GameObject[] gameTypePrefabs;
    LoaderBase currentGameTypeLoader;
    IResultMatch resultatorMatch;

    #endregion

    void Awake()
    {
        GM = this;
    }

    public LoaderBase GetGameTypeLoader()
    {
        return currentGameTypeLoader;
    }

    public void ResultMatch()
    {
        resultatorMatch.ResultMatch();
    }

    public void RandomizeGameType()
    {
        int randomNumber = Random.Range(0, Enum.GetNames(typeof(GameType)).Length);
        //gameTypePrefabs[randomNumber] VMESTO gameTypePrefabs[0]
        GameObject gameTypeObj = Instantiate(gameTypePrefabs[0]);
        currentGameTypeLoader = gameTypeObj.GetComponent<GameTypeBase>().loader;
        resultatorMatch = gameTypeObj.GetComponent<IResultMatch>();

        //gameType = (GameType)randomNumber;
        gameType = GameType.Survival;

    }
}
