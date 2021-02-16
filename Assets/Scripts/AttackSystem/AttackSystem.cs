using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSystem : MonoBehaviour
{
    public abstract void Attack(int damage, float weaponLength, Weapon weapon);
    public abstract void Block();
}
