using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackState", menuName = "EnemyStatements/Attack")]
public class Attack : State
{
    public override void Init()
    {
        Debug.Log("Attack!");
        enemy.animator.SetTrigger("Attack");
        isFinished = true;
    }
    public override void Run()
    {

    }

}
