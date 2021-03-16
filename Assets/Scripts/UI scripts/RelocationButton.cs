using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RelocationButton : MonoBehaviour
{
    public int indexPoint;
    public Button myButton;
    public void Relocate(){
        Relocation relocation = GameMap.GM.relocation;
        relocation.RelocateTo(indexPoint);
    }
}
