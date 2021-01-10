using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuPresenter : MonoBehaviour
{
    public static MenuPresenter instance;

    [SerializeField] int menuIndex;
    [SerializeField] GameObject[] gamePanels;
    [SerializeField] GameObject activePanel;


    void Awake()
    {
        instance = this;
    }

    public void ActivatePanel(int index)
    {
        activePanel.SetActive(false);
        activePanel = gamePanels[index];
        gamePanels[index].SetActive(true);

        if(index == menuIndex)
        {
            //Activate
        }
        else
        {
            //Deactivate
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
        PlayerUI UI = PlayerUI.instance;
        if (UI.menuCanvas.activeSelf)
        {
            UI.menuCanvas.SetActive(false);
            UI.gameCanvas.SetActive(true);

            return;
        }
        if (UI.gameCanvas.activeSelf)
        {
            Time.timeScale = 1f;
            UI.menuCanvas.SetActive(true);

            if(UI.pauseMenu.activeSelf)
                UI.pauseMenu.SetActive(false);

            UI.gameCanvas.SetActive(false);
            return;
        }
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
        PlayerUI.instance.pauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PlayerUI.instance.pauseMenu.SetActive(false);
    }
}
