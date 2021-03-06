using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CarrotAttack", menuName = "EnemyStatements/Carrot/Attack")]
public class CarrotAttack : State
{
    [SerializeField] float attackDelay;
    [SerializeField] GameObject carrotThornPref;
    public override void Init(){
        timer = attackDelay;
        enemy.animator.SetTrigger("Attack");
    }
    float timer;
    public override void Run()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f){
            foreach (var target in enemy.TargetsInMyViewZone)
            {
                Vector3 targetPosition = target.transform.transform.position;
                GameObject thorn = Instantiate(carrotThornPref, targetPosition, Quaternion.identity);
                Debug.Log("))))");
            }
            Debug.Log("22222");
            isFinished = true;
        }
    }
}
