using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckPoint", menuName = "CreateProp/CheckPoint")]
public class CheckPointProp : Prop
{
    public override void UseProp(){
        Relocation relocation = GameMap.GM.relocation;
        relocation.SetRelocationPanel();
    }
}
