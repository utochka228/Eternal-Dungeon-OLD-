using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotEnemy : Enemy
{
    [SerializeField] State attackState;
    [SerializeField] State idleState;
    new void Start()
    {
        base.Start();
    }

    public override void DoStatementWork(){
        if(!currentState.isFinished){
            currentState.Run();
            Debug.Log("33333");
            string currentStateName = currentState.GetType().Name;
            string attackName = attackState.GetType().Name;
            if(TargetsInMyViewZone.Count > 0 && currentStateName != attackName){
                Debug.Log("!!!!!!");
                SetState(attackState);
        }
        }else{
            string oldStateName = currentState.GetType().Name;
            if(oldStateName == "CarrotAttack"){
                SetState(idleState);
            }
        }
    }
}
