using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolState", menuName = "EnemyStatements/Patrol")]
public class Patrol : State
{
    Vector2 targetA;
    Vector2 targetB;
    Vector2 currentTarget;

    bool targetWasFound;

    public override void Init()
    {
       // FindTarget();
    }

    public override void Run()
    {
        if (enemy.transform.position == new Vector3(currentTarget.x, currentTarget.y, enemy.transform.position.z))
            currentTarget = currentTarget == targetA ? targetB : targetA;

        if (targetWasFound)
            FollowTarget();
    }

    public override void Exit()
    {

    }
    Vector2 patrolPosition = -Vector2.one;

    // void FindTarget()
    // {
    //     Vector2 currentPosition = enemy.currentPosition;
    //     targetA = currentPosition;
    //     Vector2[] directions = {
    //         new Vector2(0, 1),
    //         new Vector2(1, 0),
    //         new Vector2(0, -1),
    //         new Vector2(-1, 0)
    //     };

    //     for (int i = 0; i < directions.Length; i++)
    //     {
    //         Vector2 tempPos = GameMap.GM.GetLastPointInSelectedDirection(currentPosition, directions[i]);
    //         Debug.Log($"tempPos: {tempPos} with direction {directions[i]}");
    //         if (i == 0)
    //         {
    //             patrolPosition = tempPos;
    //             continue;
    //         }

    //         if (Vector2.Distance(currentPosition, tempPos) > Vector2.Distance(currentPosition, patrolPosition))
    //             patrolPosition = tempPos;
    //     }

    //     Debug.Log($"PatrolState -> targetB: {patrolPosition.x} | {patrolPosition.y}");
    //     targetB = patrolPosition;
    //     currentTarget = targetB;

    //     targetWasFound = true;
    // }
    void FollowTarget()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(currentTarget.x, currentTarget.y, enemy.transform.position.z), 4f * Time.deltaTime);
    }
}
