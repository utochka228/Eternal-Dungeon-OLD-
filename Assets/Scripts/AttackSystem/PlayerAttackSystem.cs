using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : AttackSystem
{
    public Transform attackPoint;
    [SerializeField] float radius = 3f;
    public override void Attack(float damage)
    {
        int maskPlayer = ~LayerMask.GetMask("Player");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, radius, maskPlayer);
        foreach (var collider in colliders)
        {
            
            IDamageble hit = collider.GetComponent<IDamageble>();
            if(hit != null){
                hit.TakeDamage(gameObject, damage);
            }
        }
    }

    public override void Block()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
