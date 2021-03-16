using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "CreateItem/Food")]
public class Food : Item
{
    public int healthRestoration;

    public override string GetItemType()
    {
        return this.GetType().Name;
    }

    public override void UseItem(Transform user)
    {
        IHealable userHealable = user.GetComponent<IHealable>();
        if(userHealable != null)
            userHealable.Heal(healthRestoration);
    }
}
