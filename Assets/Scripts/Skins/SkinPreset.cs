using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinPreset", menuName = "Skin/CreateSkinPreset", order = 1)]
public class SkinPreset : ScriptableObject
{
    public string skinName = "Default";
    public int skinCost = 10;
}
