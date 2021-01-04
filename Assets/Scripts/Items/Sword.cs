using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSwordType", menuName = "CreateItem/Sword")]
public class Sword : Item
{
    float damage = 2f;
    float swordLength = 1f;
    float speed = 0.1f;

    [HideInInspector] public bool isJabWeapon;
    [HideInInspector] public AnimationClip JabAnimation;
    [HideInInspector] public bool isSwingWeapon;
    [HideInInspector] public AnimationClip SwingAnimation;
    public override void UseItem(Transform user)
    {
        //Activate sword animation

        //Activate player animation

        //Attack (AttackSystem)
        PlayerAttackSystem attackSystem = user.GetComponent<PlayerAttackSystem>();
        attackSystem?.Attack(damage);
    }
    
}
