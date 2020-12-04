using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public override void UseItem(Transform user)
    {
        PlayerController pc = user.GetComponent<PlayerController>();

        if(pc != null)
        {
            GameTypeBase.instance.moneyTrophy++;
            Destroy(gameObject);
        }
    }
}
