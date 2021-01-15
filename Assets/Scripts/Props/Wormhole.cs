using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tunnel", menuName = "CreateProp/Tunnel")]
public class Wormhole : Prop
{
    public override void UseProp(){
        Relocation relocation = GameMap.GM.relocation;
        relocation.ChangeLevel();
        Debug.Log("EXITUSED");
    }
}
