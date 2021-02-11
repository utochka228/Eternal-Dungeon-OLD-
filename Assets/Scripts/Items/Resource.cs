using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewResource", menuName = "CreateItem/Resource")]
public class Resource : Item
{
    public override string GetItemType()
    {
        return this.GetType().Name;
    }

    public override void UseItem(Transform user)
    {
        throw new System.NotImplementedException();
    }

    
}
