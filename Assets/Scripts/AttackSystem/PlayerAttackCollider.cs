using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    int damage;
    string weaponType;
    [SerializeField] CircleCollider2D attackCollider;
    void OnTriggerEnter2D(Collider2D other) {
        Block block = other.GetComponent<Block>();
        if(weaponType != "PickAxe" && block != null)
            return;
        IDamageble hit = other.GetComponent<IDamageble>();
        if(hit != null){
            hit.TakeDamage(GameSession.instance.Player.gameObject, damage);
        }
    }
    public void SetAttackData(int damage, string weaponType, float colliderRadius){
        this.damage = damage;
        this.weaponType = weaponType;
        attackCollider.radius = colliderRadius;
    }
    public void EnableCollider(){
        attackCollider.enabled = true;
    }
    public void DisableCollider(){
        attackCollider.enabled = false;
    }
}
