using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RelocationButton : MonoBehaviour
{
    public int indexPoint;
    public int relocationEnergy;
    
    public void Relocate(){
        Relocation relocation = GameMap.GM.relocation;
        relocation.RelocateToCheckPoint(indexPoint, relocationEnergy);
    }
}
