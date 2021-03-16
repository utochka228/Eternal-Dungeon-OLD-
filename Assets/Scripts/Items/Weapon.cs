using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item, IEquipable
{
    [ToolTipInfo("Damage per hit")]
    [SerializeField] public int damage = 2;
    [ToolTipInfo("Attack Speed")]
    [SerializeField] public float speed = 0.1f;
    [ToolTipInfo("Critical chance")]
    [Range(0f, 1f)] public float criticalChance;
    [ToolTipInfo("Weapon length")]
    public float colliderRadius;
    public Vector3 attackPointOffset;
    public AnimatorTriggerName animatorTriggerName;
    public WeaponHands weaponHands;

    public void Equip()
    {
        Inventory inventory = Inventory.instance;
        inventory.SetHandItem(this);
        inventory.handSpriteHolder.sprite = sprite;
        inventory.handSpriteHolder.transform.localPosition = spawnOffset;
        inventory.handSpriteHolder.transform.localRotation = Quaternion.Euler(0, 0, spawnRotation);
    }

    public void Unequip()
    {
        Inventory inventory = Inventory.instance;
        inventory.handSpriteHolder.sprite = null;
        inventory.SetHandItem(null);
        inventory.playerAnimator.SetLayerWeight(1, 0f);
        inventory.playerAnimator.SetLayerWeight(2, 0f);
    }
}
public enum WeaponHands { OneHand, TwoHand}
public enum AnimatorTriggerName { Attack_Swing, Attack_Stabb}