using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public enum EnemyStatement { Idle, Patrol, Escape, Chase, Attack}

    public EnemyStatement statement;

    public bool isFinished { get; protected set; }
    public bool runNext { get; protected set; }
    //State will be Run next after this (if runNext == true)
    public State NextState;
    //gameplay parameters
    [HideInInspector] public Enemy enemy;

    public virtual void Init() { }

    //Simulation method "Update" by calling method "Run" somewhere in Update in any script 
    public abstract void Run();
    public virtual void Exit() { }
}
