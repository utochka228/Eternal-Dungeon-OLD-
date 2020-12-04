using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsUpgrader : MonoBehaviour
{
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private TextMeshProUGUI costToBuyText;
    [SerializeField]
    private TextMeshProUGUI statProgress;
    [SerializeField]
    private int costToBuy;
    [SerializeField]
    private Stat myStatToUpgrade;
    [SerializeField]
    private GameObject toolTip;

    [SerializeField]
    private int indexInSave;

    // Start is called before the first frame update
    void Start()
    {
        MenuManager.MG.OnSaving += SaveProgress;

        if (PlayerPrefs.HasKey("Saves"))
        {
            LoadProgress();
        }
        
        costToBuyText.text = costToBuy.ToString();
        statProgress.text = progressSlider.value + "/" + progressSlider.maxValue;
    }
    void SaveProgress()
    {
        MenuManager.MG.savingData.sliderProgress[indexInSave] = (int)progressSlider.value;
        MenuManager.MG.savingData.moneyCost[indexInSave] = costToBuy;
    }
    void LoadProgress()
    {
        SavingData data = MenuManager.MG.savingData;
        progressSlider.value = data.sliderProgress[indexInSave];
        statProgress.text = progressSlider.value + "/" + progressSlider.maxValue;
        costToBuy = data.moneyCost[indexInSave];
        costToBuyText.text = costToBuy.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyUpgrade()
    {
        int currentPlayerMoney = PlayerInfo.PI.Money;

        if (currentPlayerMoney >= costToBuy && progressSlider.value < progressSlider.maxValue)
        {
            //Upgrade stat
            myStatToUpgrade.UpgradeStat();
            progressSlider.value++;
            PlayerInfo.PI.Money -= costToBuy;
            statProgress.text = progressSlider.value + "/" + progressSlider.maxValue;
            CalculateCost();
            Debug.Log("UPGRADING!");
        }
        else return;
    }

    void CalculateCost()
    {
        //CAlculation
        int newPrice = costToBuy * 2;
        costToBuy = newPrice;
        costToBuyText.text = costToBuy.ToString();
    }

    public void ActivateToolTip()
    {
        toolTip.SetActive(!toolTip.activeSelf);
    }

}
