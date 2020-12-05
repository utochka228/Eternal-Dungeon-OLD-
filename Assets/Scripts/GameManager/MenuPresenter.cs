using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MenuPresenter : MonoBehaviour
{
    public static MenuPresenter instance;

    #region PublicVariables
    
    #endregion

    #region PrivateVariables

    [SerializeField]
    private int menuIndex;

    [SerializeField]
    private GameObject[] gamePanels;

    [SerializeField]
    private GameObject activePanel;
    [SerializeField]
    private GameObject menuCanvas;
    public GameObject gameCanvas;
    [SerializeField]
    private ParticleSystem starsEffect;
    
    [SerializeField]
    private GameObject pauseMenu;
    
    [SerializeField]
    private GameObject countDownPrefab;

    
    public GameObject congratulations;


    #endregion
       void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PlayerInfo.PI.Expirience += 5f;
    }

    public void ActivatePanel(int index)
    {
        activePanel.SetActive(false);
        activePanel = gamePanels[index];
        gamePanels[index].SetActive(true);

        if(index == menuIndex)
        {
            //Activate
            starsEffect.Play();
        }
        else
        {
            //Deactivate
            starsEffect.Stop();
        }
    }


    public void ActiveMenuPanel()
    {
        activePanel.SetActive(false);
        activePanel = gamePanels[menuIndex];
        gamePanels[menuIndex].SetActive(true);
    }

    public void HideOtherUI()
    {
        if (menuCanvas.activeSelf)
        {
            menuCanvas.SetActive(false);

            starsEffect.Stop();

            if(GameLoader.GL != null)
                Destroy(GameLoader.GL.gameObject, 4f);

            return;
        }
        if (gameCanvas.activeSelf)
        {
            Time.timeScale = 1f;
            menuCanvas.SetActive(true);

            if(pauseMenu.activeSelf)
                pauseMenu.SetActive(false);


            starsEffect.Play();

            gameCanvas.SetActive(false);
            return;
        }
    }

   

   

    public void SpawnCountDown()
    {
        GameObject obj = Instantiate(countDownPrefab, gameCanvas.transform);
        StartCoroutine(WaitSecondsAfterGameLoader(obj));
    }

    IEnumerator WaitSecondsAfterGameLoader(GameObject countDownPrefab)
    {
        yield return new WaitForSecondsRealtime(4f);
        Destroy(countDownPrefab);
        SetTimeScaleToNormal();
        GameTypeBase.instance.OnMatchStarted();
    }

    public void SetTimeScaleToNormal()
    {
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
