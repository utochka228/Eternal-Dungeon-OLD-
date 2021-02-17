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

    public void Die(GameObject murderer)
    {
        Dying(murderer);
    }

    private void Update() {

    }

    public void TakeDamage(GameObject hitter, int damage, bool isCritical)
    {
        TakingDamage(hitter, damage, isCritical);
    }
    protected abstract void Dying(GameObject murderer);
    protected abstract void TakingDamage(GameObject hitter, int damage, bool isCritical);

}
