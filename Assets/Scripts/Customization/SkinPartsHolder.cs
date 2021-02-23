using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPartsHolder : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer beard;
    public SpriteRenderer hair;
    public SpriteRenderer armL;
    public SpriteRenderer armR;
    public SpriteRenderer chest;
    public SpriteRenderer handL;
    public SpriteRenderer handR;
    public SpriteRenderer legL;
    public SpriteRenderer legR;
    
    public void UpdateSkinLook(SkinPreset preset){
        head.sprite = preset.headType;
        head.color = preset.skinColor;
        beard.sprite = preset.beardType;
        beard.color = preset.beardColor;
        hair.sprite = preset.hairType;
        hair.color = preset.hairColor;
        armL.color = preset.armsColor;
        armR.color = preset.armsColor;
        chest.sprite = preset.chestType;
        chest.color = preset.chestColor;
        handL.color = preset.skinColor;
        handR.color = preset.skinColor;
        legL.color = preset.legsColor;
        legR.color = preset.legsColor;
    }
}
