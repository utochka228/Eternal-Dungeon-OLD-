using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSwordType", menuName = "CreateItem/Sword")]
public class Sword : Weapon
{
    [SerializeField] float swordLength = 1f;
    [HideInInspector] public bool isJabWeapon;
    [HideInInspector] public bool isSwingWeapon;
    public override void UseItem(Transform user)
    {
        PlayerAttackSystem attackSystem = user.GetComponent<PlayerAttackSystem>();
        attackSystem?.Attack(damage, swordLength, this);
    }

    public override string GetItemType()
    {
        return "Weapon";
    }
}
