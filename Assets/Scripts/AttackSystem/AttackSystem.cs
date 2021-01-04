using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSystem : MonoBehaviour
{
    public abstract void Attack(float damage);
    public abstract void Block();
}
