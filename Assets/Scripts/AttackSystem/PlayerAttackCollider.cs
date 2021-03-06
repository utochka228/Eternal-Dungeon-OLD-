using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    int damage;
    string weaponType;
    bool isCritical;
    [SerializeField] CircleCollider2D attackCollider;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.isTrigger == true)
            return;
        Block block = other.GetComponent<Block>();
        if(weaponType != "PickAxe" && block != null)
            return;
        IDamageble hit = other.GetComponent<IDamageble>();
        Debug.Log("IS CRIT:" + isCritical);
        if(hit != null){
            hit.TakeDamage(GameSession.instance.Player.gameObject, damage, isCritical);
        }
    }
    public void SetAttackData(int damage, bool isCritical, string weaponType, float colliderRadius){
        this.isCritical = isCritical;
        Debug.Log("IS CRIT:" + isCritical);
        if(this.isCritical)
            this.damage = damage*2;
        else
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
