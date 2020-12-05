using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameLoader : MonoBehaviour
{
    public static GameLoader GL;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Image gameTypeIcon;
    [SerializeField]
    private TextMeshProUGUI tipText;
    [SerializeField]
    private TextMeshProUGUI gameTypeName;

    LoaderBase loader;
    void Awake()
    {
        GL = this;
    }

    void Start()
    {
        loader = GameType.instance.GetGameTypeLoader();
        SetBackGroundColor(loader.backColor);
        SetGameIcon(loader.gameLoaderSprite);
        SetTipText(loader.gameLoaderTipText);
        SetGameTypeNameText(loader.gameTypeName);
    }

    public void SetGameIcon(Sprite sprite)
    {
        gameTypeIcon.sprite = sprite;
    }
    public void SetTipText(string text)
    {
        tipText.text = text;
    }
    public void SetGameTypeNameText(string text)
    {
        gameTypeName.text = text;
    }
    public void SetBackGroundColor(Color color)
    {
        background.color = color;
    }

    void OnDestroy()
    {
        MenuPresenter.instance.gameCanvas.SetActive(true);
        Time.timeScale = 0f;
        //LAUNCH COUNTDOWN
        MenuPresenter.instance.SpawnCountDown();
    }
}
