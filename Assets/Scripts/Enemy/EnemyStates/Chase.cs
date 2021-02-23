using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "EnemyStatements/Chase")]
public class Chase : State
{
    Transform target;
    public override void Init()
    {
        target = enemy.GetClosestVisibleTarget().transform;
        enemy.SetTarget(target);
    }

    public override void Run()
    {
        Vector2 myPos = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        float distance = Vector2.Distance(myPos, targetPos);
        if(distance <= 1f){
            isFinished = true;
        }
    }

}
