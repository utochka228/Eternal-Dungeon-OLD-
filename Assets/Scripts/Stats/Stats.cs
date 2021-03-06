using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IDamageble
{
    void TakeDamage(GameObject hitter, int damage, bool isCritical);
    void Die(GameObject murderer);
}
public abstract class Stats : MonoBehaviour, IDamageble
{
    [SerializeField]
    protected int health = 3;
    [SerializeField]
    protected int maxHealth;
    public abstract int Health { get; set; }
    public virtual int MaxHealth{ get; set;}
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
    protected abstract void Dying(GameObject murderer);
    protected virtual void TakingDamage(GameObject hitter, int damage, bool isCritical){
        centerOfBody = GetComponent<Collider2D>().bounds.center;
        DamagePopup.Create(centerOfBody, damage, isCritical);
    }

}
