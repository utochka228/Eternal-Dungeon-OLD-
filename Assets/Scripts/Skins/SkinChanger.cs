using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public static SkinChanger skinChanger;
    [SerializeField]
    private SkinPreset[] skinPresets;
    [SerializeField]
    private GameObject[] skins;
    [SerializeField]
    private GameObject currentActiveSkin;

    public int currentActiveSkinIndex;

    void Awake()
    {
        skinChanger = this;
    }

    public void BuySkin(int index)
    {
        if(PlayerInfo.PI.Money >= skinPresets[index].skinCost)
        {
            Debug.Log("Bought skin: " + skinPresets[index]);
            PlayerInfo.PI.Money -= skinPresets[index].skinCost;
        }
    }

    public void EquipSkin(int index)
    {
        currentActiveSkin.SetActive(false);
        currentActiveSkin = skins[index];
        currentActiveSkinIndex = index;
        currentActiveSkin.SetActive(true);
    }
}
