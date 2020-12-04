using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : AttackSystem
{
    [SerializeField] GameObject fireBall;
    [SerializeField] float power;
    [SerializeField] Vector2 direction;
    public override void Attack()
    {
        Transform fBall = Instantiate(fireBall).transform;
        fBall.position = transform.position;
        fBall.GetComponent<Rigidbody2D>().AddForce(direction*power*Time.deltaTime);
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
