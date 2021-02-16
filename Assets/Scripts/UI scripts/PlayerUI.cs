using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject pauseMenu;
    public GameObject congratulations;
    public HealthHearts playerHearts;

    public FixedJoystick joystick;
    public Button interactButton;
    public Button attackButton;
    public TextMeshProUGUI interactName;
    public TextMeshProUGUI menuMoneyText;
    public TextMeshProUGUI storeMoneyText;
    public Slider expSlider;
    public TextMeshProUGUI lvlText;

    public GameObject relocationPanel;
    public GameObject relocationHolder;
    public GameObject relocationButton;

    public Transform inventorySlotHolder;
    public Transform inventoryPanel;

    public GameObject shopPanel;
    void Awake()
    {
        instance = this;
    }

    void Start(){
        storeMoneyText.text = PlayerInfo.PI.Money.ToString();
        menuMoneyText.text = PlayerInfo.PI.Money.ToString();
    }
    public void EnableDisableInventory(){
        bool activate = !inventoryPanel.gameObject.activeSelf;
        inventoryPanel.gameObject.SetActive(activate);
    }
}
