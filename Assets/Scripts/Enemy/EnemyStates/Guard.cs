using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuardState", menuName = "EnemyStatements/Guard")]
public class Guard : State
{
    GuardianEnemy myGuardEnemy;   
    Transform myGuardTarget;
    public override void Init()
    {
        myGuardEnemy = enemy.GetComponent<GuardianEnemy>();
        myGuardTarget = myGuardEnemy.guardPoint;
        enemy.SetTarget(myGuardTarget); 
    }
    public override void Run()
    {
        if(enemy.TargetsInMyViewZone.Count > 0){
            isFinished = true;
        }
    }
}
