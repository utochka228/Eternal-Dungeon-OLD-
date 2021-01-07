using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSystem : MonoBehaviour
{
    public abstract void Attack(float damage, float weaponLength, string weaponType);
    public abstract void Block();
}
