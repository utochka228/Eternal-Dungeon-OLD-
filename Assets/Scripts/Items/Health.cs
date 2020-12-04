using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Item
{
    public override void UseItem(Transform user)
    {
        Stats stats = user.GetComponent<Stats>();

        if (stats != null)
        {
            stats.Health++;
            Debug.Log($"{user.name} healed!");
            //player.ActivateFloatingText(1, player.floatingHealthText);
            Destroy(gameObject);
        }
    }
}
