using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "PlayerInfo/Create")]
public class PlayerInfo : ScriptableObject
{
    [SerializeField]
    private float maxExpValueChanger;

    #region PlayerData
    public bool newCharacter = true;
    private int lvl;

    public int Level
    {
        get{return lvl;}
            
        set
        {
            if (value >= 0)
            {
                lvl = value;
                PlayerUI.instance.congratulations.SetActive(true);
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

}
