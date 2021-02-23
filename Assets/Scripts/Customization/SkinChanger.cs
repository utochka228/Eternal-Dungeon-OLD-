using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HSVPicker;
using TMPro;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public SkinPartsHolder skinHolder;
    public SkinPreset skinPreset;
    [SerializeField] BodyPart[] bodyParties;
    [SerializeField] ColorPicker colorPicker;
    GameObject clrPicker;
    [SerializeField] GameObject typeSelector;
    [SerializeField] TextMeshProUGUI typeHeader;
    void Start()
    {
        clrPicker = colorPicker.gameObject;
        clrPicker.SetActive(false);
        typeSelector.SetActive(false);
    }
    Sprite[] loadedSprites;
    int currentSpriteIndex = 0;
    string currentPart;
    public void ChangePlayerPart(string partName){
        clrPicker.SetActive(false);
        typeSelector.SetActive(false);
        currentPart = partName;
        colorPicker.onValueChanged.RemoveListener(ChangePartColor);
        var bodyPart = bodyParties.Single(x => x.partName == partName);
        if(bodyPart.changeType){
            typeSelector.SetActive(true);
            loadedSprites = Resources.LoadAll<Sprite>("Skin/" + partName);
            typeHeader.text = loadedSprites[0].name;
        }
        if(bodyPart.changeColor){
            clrPicker.SetActive(true);
            colorPicker.onValueChanged.AddListener(ChangePartColor);
        }
    }
    public void SelectNextType(){
        if(loadedSprites.Length == 0)
            return;
        int newIndex = currentSpriteIndex+1;
        if(newIndex == loadedSprites.Length)
            newIndex = 0;
        Sprite selectedSprite = loadedSprites[newIndex];
        typeHeader.text = selectedSprite.name;
        ChangePartType(selectedSprite);
    }
    public void SelectPrevType(){
        if(loadedSprites.Length == 0)
            return;
        int newIndex = currentSpriteIndex-1;
        if(newIndex == -1)
            newIndex = loadedSprites.Length-1;
        Sprite selectedSprite = loadedSprites[newIndex];
        typeHeader.text = selectedSprite.name;
        ChangePartType(selectedSprite);
    }
    void ChangePartType(Sprite sprite){
        if(currentPart == "Hair"){
            skinHolder.hair.sprite = sprite;
        }
        if(currentPart == "Head"){
            skinHolder.head.sprite = sprite;
        }
        if(currentPart == "Beard"){
            skinHolder.beard.sprite = sprite;
        }
        if(currentPart == "Chest"){
            skinHolder.chest.sprite = sprite;
        }
    }
    void ChangePartColor(Color color){
        if(currentPart == "Skin"){
            skinHolder.head.color = color;
            skinHolder.handL.color = color;
            skinHolder.handR.color = color;
        }
        if(currentPart == "Hair"){
            skinHolder.hair.color = color;
        }
        if(currentPart == "Beard"){
            skinHolder.beard.color = color;
        }
        if(currentPart == "Chest"){
            skinHolder.chest.color = color;
        }
        if(currentPart == "Arms"){
            skinHolder.armL.color = color;
            skinHolder.armR.color = color;
        }
        if(currentPart == "Legs"){
            skinHolder.legL.color = color;
            skinHolder.legR.color = color;
        }
    }
    public void DisableHair(){skinHolder.hair.sprite = null; }
    public void DisableBeard(){skinHolder.beard.sprite = null; }

    public void SaveSkinPreset(){
        skinPreset.headType = skinHolder.head.sprite;
        skinPreset.skinColor = skinHolder.head.color;
        skinPreset.hairType = skinHolder.hair.sprite;
        skinPreset.hairColor = skinHolder.hair.color;
        skinPreset.beardType = skinHolder.beard.sprite;
        skinPreset.beardColor = skinHolder.beard.color;
        skinPreset.chestType = skinHolder.chest.sprite;
        skinPreset.chestColor = skinHolder.chest.color;
        skinPreset.armsColor = skinHolder.armR.color;
        skinPreset.legsColor = skinHolder.legL.color;
    }
}
[System.Serializable]
struct BodyPart{
    public string partName;
    public bool changeType;
    public bool changeColor;
}
