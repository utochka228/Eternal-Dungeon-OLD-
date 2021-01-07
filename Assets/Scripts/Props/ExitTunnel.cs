using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tunnel", menuName = "CreateProp/Tunnel")]
public class ExitTunnel : Prop
{
    public override void UseProp(){
        IRelocation relocation = GameTypeBase.instance.transform.GetComponent<IRelocation>();
        relocation.ChangeLevel();
        Debug.Log("EXITUSED");
    }
}
