using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;

    [SerializeField]
    public TextMeshProUGUI playerHealth;

    public FixedJoystick joystick;
    public Button attackButton;

    public TextMeshProUGUI menuMoneyText;
    public TextMeshProUGUI storeMoneyText;
    public Slider expSlider;
    public TextMeshProUGUI lvlText;

    public GameObject relocationPanel;
    public GameObject relocationButton;
    void Awake()
    {
        instance = this;
    }

    void Start(){
        storeMoneyText.text = PlayerInfo.PI.Money.ToString();
        menuMoneyText.text = PlayerInfo.PI.Money.ToString();
    }
}
