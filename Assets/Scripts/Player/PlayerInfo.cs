using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main player information for all Game
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    [SerializeField]
    private float maxExpValueChanger;

    #region PlayerData

    private int lvl;

    public int Level
    {
        get
        {
            return lvl;
        }
        set
        {
            if (value >= 0)
            {
                lvl = value;
                MenuManager.MG.congratulations.SetActive(true);
                MenuManager.MG.lvlText.text = lvl.ToString();
            }
        }
    }

    private float playerExp;

    public float Expirience
    {
        get
        {
            return playerExp;
        }
        set
        {
            if (value >= 0)
            {
                playerExp = value;
                if(playerExp >= MenuManager.MG.expSlider.maxValue)
                {
                    Level++;

                    float remainder = MenuManager.MG.expSlider.maxValue - playerExp;
                    float oldMaxValue = MenuManager.MG.expSlider.maxValue;

                    MenuManager.MG.expSlider.maxValue += maxExpValueChanger;

                    Expirience = remainder;
                }
                else
                    MenuManager.MG.expSlider.value = playerExp;
            }
        }
    }

    private int money;
    public int Money
    {
        get
        {
            return money;
        }

        set
        {
            if (value >= 0)
            {
                money = value;
                MenuManager.MG.menuMoneyText.text = money.ToString();
                MenuManager.MG.storeMoneyText.text = money.ToString();
            }
        }
    }

    private int healthCount = 1;

    public int Health
    {
        get
        {
            return healthCount;
        }
        set
        {
            if(value >= 0)
            {
                healthCount = value;
            }
        }
    }

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        PI = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Money += 100;
    }
}
