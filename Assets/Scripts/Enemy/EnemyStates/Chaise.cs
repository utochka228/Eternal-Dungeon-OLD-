using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaiseState", menuName = "EnemyStatements/Chaise")]
public class Chaise : State
{
    PlayerController pController;
    public override void Init()
    {
        //enemy.movementScript.OnPathNotSuccessful += Chaising;
        Chaising();
        pController = enemy.target.GetComponent<PlayerController>();
        pController.OnChangedPlayerPosition += Chaising;
    }

    public override void Run()
    {
        
    }

    public override void Exit()
    {
        //enemy.movementScript.OnPathNotSuccessful -= Chaising;
        pController.OnChangedPlayerPosition -= Chaising;
    }

    void Chaising()
    {
        Debug.Log("Chaising!");

        if(enemy.target != null)
        {
            Transform target = enemy.target;
            Vector2 targetPosition = new Vector2(target.position.x, target.position.y);

            enemy.MoveToTarget(enemy.currentPosition, targetPosition);
        }
    }
}
