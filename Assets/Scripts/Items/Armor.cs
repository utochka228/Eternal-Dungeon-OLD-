using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Armor", menuName = "CreateItem/Armor")]
public class Armor : Item, IEquipable
{
    public ArmorType armorType;
    public int defence;
    public void Equip()
    {
        Inventory inventory = Inventory.instance;
        switch (armorType)
        {
            case ArmorType.Head:
                inventory.SetHeadItem(this);
                inventory.headSpriteHolder.sprite = sprite;
            break;
            case ArmorType.Chest:
                inventory.SetChestItem(this);
                inventory.chestSpriteHolder.sprite = sprite;
            break;
            case ArmorType.Legs:
                inventory.SetLegsItem(this);
                inventory.LegsSpriteHolder = sprite;
            break;
        }
        PlayerStats stats = inventory.GetComponent<PlayerStats>();
        stats.Defence += defence;
    }

    public void Unequip()
    {
        Inventory inventory = Inventory.instance;
        switch (armorType)
        {
            case ArmorType.Head:
                inventory.headSpriteHolder.sprite = null;
                inventory.SetHeadItem(null);
                //return hair
            break;
            case ArmorType.Chest:
                inventory.chestSpriteHolder.sprite = null;
                inventory.SetChestItem(null);
            break;
            case ArmorType.Legs:
                inventory.LegsSpriteHolder = null;
                inventory.SetLegsItem(null);
                //return default legs
            break;
        }
        PlayerStats stats = inventory.GetComponent<PlayerStats>();
        stats.Defence -= defence;
    }
}
public enum ArmorType{ Head, Chest, Legs}