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
                MenuPresenter.instance.congratulations.SetActive(true);
                PlayerUI.instance.lvlText.text = lvl.ToString();
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
                if(playerExp >= PlayerUI.instance.expSlider.maxValue)
                {
                    Level++;

                    float remainder = PlayerUI.instance.expSlider.maxValue - playerExp;
                    float oldMaxValue = PlayerUI.instance.expSlider.maxValue;

                    PlayerUI.instance.expSlider.maxValue += maxExpValueChanger;

                    Expirience = remainder;
                }
                else
                    PlayerUI.instance.expSlider.value = playerExp;
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
                PlayerUI.instance.menuMoneyText.text = money.ToString();
                PlayerUI.instance.storeMoneyText.text = money.ToString();
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

}
