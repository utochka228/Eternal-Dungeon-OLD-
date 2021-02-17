using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : AttackSystem
{
    [SerializeField] Transform attackPoint;
    [SerializeField] PlayerAttackCollider attackCollider;
    Animator playerAnimator;
    public override void Attack(int damage, float weaponLength, Weapon weapon)
    {
        WeaponHands weaponHands = weapon.weaponHands;
        if(weaponHands == WeaponHands.OneHand){
            playerAnimator.SetTrigger(weapon.animatorTriggerName.ToString());
        }
        if(weaponHands == WeaponHands.TwoHand){
            playerAnimator.SetTrigger(weapon.animatorTriggerName.ToString());
        }
        bool isCritical = IsCriticalHit(weapon.criticalChance);
        attackCollider.SetAttackData(damage, isCritical, weapon.GetItemType(), weapon.colliderRadius);
    }
    bool IsCriticalHit(float weaponCritChance){
        if(Random.value < weaponCritChance)
            return true;
        else
            return false;
    }
    public void EnableAttackCollider(){
        attackCollider.EnableCollider();
    }
    public void DisableAttackCollider(){
        attackCollider.DisableCollider();
    }

    public override void Block()
    {
        
    }

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

}
