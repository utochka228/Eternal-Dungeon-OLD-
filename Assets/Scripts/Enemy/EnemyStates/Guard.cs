using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuardState", menuName = "EnemyStatements/Guard")]
public class Guard : State
{
    Vector2 guardPoint;

    public override void Init()
    {
        
    }
    public override void Run()
    {
        //enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(currentTarget.x, currentTarget.y, enemy.transform.position.z), 4f * Time.deltaTime);
    }
}
