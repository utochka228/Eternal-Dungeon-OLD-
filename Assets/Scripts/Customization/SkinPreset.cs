using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlayerLook/CreateLook", fileName = "PlayerLook/")]
public class SkinPreset : ScriptableObject
{
    [Header("Head")]
    public Sprite headType;
    public Sprite beardType;
    public Color beardColor;
    public Sprite hairType;
    public Color hairColor;
    [Header("Arms")]
    public Color armsColor;
    [Header("Chest")]
    public Sprite chestType;
    public Color chestColor;
    [Header("Skin color")] public Color skinColor;
    [Header("Legs color")] public Color legsColor;


    public static void UpdatePlayerLook(){

    }
}
