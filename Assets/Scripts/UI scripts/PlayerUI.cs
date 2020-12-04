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

    void Awake()
    {
        instance = this;
    }
}
