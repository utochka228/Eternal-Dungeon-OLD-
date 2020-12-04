using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianEnemy : Enemy
{
    [Header("Enemy Statements")]
    public State StartState;
    public State Guard;
    public State Guard_Attack;
    public State Chase;

    [SerializeField] float guardZoneDistance = 2f;

    new void Start()
    {
        base.Start();
        SetState(StartState);
    }

    public override void DoStatementWork()
    {
        //float distance = Vector2.Distance(guardPoint, target.position);
        if (!currentState.isFinished)
        {
            //if (distance <= guardZoneDistance)
            //    SetState(Chase);
            //else
            //    SetState(Guard);
            currentState.Run();
        }
        else
        {
            
        }
    }
}
