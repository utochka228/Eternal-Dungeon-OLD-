using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField] protected float damage = 2f;
    [SerializeField] protected float speed = 0.1f;
    public float colliderRadius;
    public Vector3 attackPointOffset;
    public AnimatorTriggerName animatorTriggerName;
    public WeaponHands weaponHands;
}
public enum WeaponHands { OneHand, TwoHand}
public enum AnimatorTriggerName { Attack_Swing, Attack_Stabb}