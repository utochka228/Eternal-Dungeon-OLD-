using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This connect UI elements with player data
public class UIPlayerUpdater : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI healthText;
    PlayerStats playerStats;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        SetUIRefs();
        UpdateAllUI();
    }

    void SetUIRefs()
    {
        healthText = PlayerUI.instance.playerHealth;
    }

    void UpdateAllUI()
    {
        healthText.text = playerStats.Health.ToString();
    }
}
