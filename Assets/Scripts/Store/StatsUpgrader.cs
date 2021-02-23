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
        
        costToBuyText.text = costToBuy.ToString();
        statProgress.text = progressSlider.value + "/" + progressSlider.maxValue;
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyUpgrade()
    {
        int currentPlayerMoney = MenuPresenter.playerInfo.Money;

        if (currentPlayerMoney >= costToBuy && progressSlider.value < progressSlider.maxValue)
        {
            //Upgrade stat
            myStatToUpgrade.UpgradeStat();
            progressSlider.value++;
            MenuPresenter.playerInfo.Money -= costToBuy;
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
