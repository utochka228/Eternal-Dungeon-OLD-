using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    [SerializeField] CircleCollider2D attackCollider;
    [SerializeField] GameObject hitter;
    [SerializeField] int damage;
    EnemyStats stats;
    void Start() {
        stats = hitter.GetComponent<EnemyStats>();  
        if(stats != null)
            damage = stats.myDamage;  
    }
    void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null){
            IDamageble targetDamage = other.GetComponent<IDamageble>();
            if(targetDamage != null)
                targetDamage.TakeDamage(hitter, damage, false);
        }
    }
    
}
