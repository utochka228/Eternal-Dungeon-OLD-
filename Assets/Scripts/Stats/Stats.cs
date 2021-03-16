using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IDamageble
{
    void TakeDamage(GameObject hitter, int damage, bool isCritical);
    void Die(GameObject murderer);
}
interface IHealable{
    void Heal(int value);
}
public abstract class Stats : MonoBehaviour, IDamageble, IHealable
{
    [SerializeField]
    protected int health = 3;
    [SerializeField]
    protected int maxHealth;
    protected int defence;
    public abstract int Health { get; set; }
    public virtual int MaxHealth{ get; set;}
    public virtual int Defence{
        get{ return defence;}
        set{
            if(value >= 0){
                defence = value;
            }
        }
    }
    Vector3 centerOfBody;
    public void Die(GameObject murderer)
    {
        Dying(murderer);
    }

    protected void Start() {
        
    }

    public void TakeDamage(GameObject hitter, int damage, bool isCritical)
    {
        TakingDamage(hitter, damage, isCritical);
    }
    public void Heal(int value)
    {
        Healing(value);
    }
    protected abstract void Dying(GameObject murderer);
    protected virtual void TakingDamage(GameObject hitter, int damage, bool isCritical){
        centerOfBody = GetComponent<Collider2D>().bounds.center;
        DamagePopup.Create(centerOfBody, damage, isCritical);
    }
    protected virtual void Healing(int value){
        centerOfBody = GetComponent<Collider2D>().bounds.center;
        DamagePopup.Create(centerOfBody, value, Color.green);
    }
}
