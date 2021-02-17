using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    [SerializeField] protected int damage = 2;
    [SerializeField] protected float speed = 0.1f;
    [Range(0f, 1f)] public float criticalChance;

    public float colliderRadius;
    public Vector3 attackPointOffset;
    public AnimatorTriggerName animatorTriggerName;
    public WeaponHands weaponHands;
}
public enum WeaponHands { OneHand, TwoHand}
public enum AnimatorTriggerName { Attack_Swing, Attack_Stabb}