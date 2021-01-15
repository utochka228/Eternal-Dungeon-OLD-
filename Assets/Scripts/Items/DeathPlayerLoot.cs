using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathPlayerLoot", menuName = "CreateItem/DeathPlayerLoot")]
public class DeathPlayerLoot : Item
{
    public List<Item> loot;
}
