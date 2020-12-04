using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    [Header("Enemy Statements")]
    public State StartState;
    public State Patrol;


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
        }
        else
        {
            if (ThisStateWasPrevious(StartState.statement)) //Idle
            {
                SetState(Patrol);
                return;
            }
        }
    }
}
