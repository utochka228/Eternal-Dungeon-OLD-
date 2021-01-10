using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RelocationButton : MonoBehaviour
{
    public int indexPoint;
    public int relocationEnergy;
    
    public void Relocate(){
        IRelocation relocation = GameSession.instance.transform.GetComponent<IRelocation>();
        relocation.RelocateToCheckPoint(indexPoint, relocationEnergy);
    }
}
