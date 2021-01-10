using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : AttackSystem
{
    public Transform attackPoint;

    public override void Attack(float damage, float weaponLength, string weaponType)
    {
        Vector2 attackPos = GameSession.instance.Player.attackPointPos;
        attackPos *= weaponLength;
        attackPos /= 2f;
        attackPoint.localPosition = new Vector3(attackPos.x, attackPos.y, attackPoint.localPosition.z);
        int maskPlayer = ~LayerMask.GetMask("Player");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, weaponLength/2, maskPlayer);
        foreach (var collider in colliders)
        {
            Block block = collider.GetComponent<Block>();
            if(weaponType != "PickAxe" && block != null)
                continue;
                
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
