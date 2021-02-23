using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianEnemy : Enemy
{
    [Header("Enemy Statements")]
    public State Guard;
    public State Attack;
    public State Chase;
    public Transform guardPoint;
    [SerializeField] float maxDistToGuardPoint = 3f;


    new void Start()
    {
        base.Start();
        SetState(StartState);
    }

    public override void DoStatementWork()
    {
        if (!currentState.isFinished)
        {
            currentState.Run();
            Vector2 guardPos = new Vector2(guardPoint.position.x, guardPoint.position.y);
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            float distance = Vector2.Distance(guardPos, myPos);
            if(distance >= maxDistToGuardPoint){
                SetState(Guard);
            }
        }
        else
        {
            string oldStateName = currentState.GetType().Name;
            if(oldStateName == "Chase"){
                SetState(Attack);
            }
            if(oldStateName == "Attack"){
                SetState(Chase);
            }
            if(oldStateName == "Guard"){
                SetState(Chase);
            }
        }
    }
}
