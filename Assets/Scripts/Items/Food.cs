using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "CreateItem/Food")]
public class Food : Item
{
    public float healthRestoration;
    public override void UseItem(Transform user)
    {
        PlayerStats stats = user.GetComponent<PlayerStats>();
        stats.Health += healthRestoration;
    }
}
