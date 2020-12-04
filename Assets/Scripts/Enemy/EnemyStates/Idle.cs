using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "EnemyStatements/Idle")]
public class Idle : State
{
    [SerializeField]
    float timeForIdling = 3f;

    float timer = 0f;

    public override void Init()
    {
        timer = timeForIdling;
    }

    public override void Run()
    {
        CountDown();
    }

    void CountDown()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            isFinished = true;
        }
    }
}
