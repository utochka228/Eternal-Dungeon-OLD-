using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IDamageble
{
    void TakeDamage(GameObject hitter, float damage);
    void Die(GameObject murderer);
}
public abstract class Stats : MonoBehaviour, IDamageble
{
    [SerializeField]
    protected float health = 3;

    public abstract float Health { get; set; }

    public void Die(GameObject murderer)
    {
        Dying(murderer);
    }

    public void TakeDamage(GameObject hitter, float damage)
    {
        TakingDamage(hitter);
    }
    protected abstract void Dying(GameObject murderer);
    protected abstract void TakingDamage(GameObject hitter);

}
