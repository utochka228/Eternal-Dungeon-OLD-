using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool isFinished { get; protected set; }
    [HideInInspector] public Enemy enemy;

    public virtual void Init() { }

    //Simulation method "Update" by calling method "Run" somewhere in Update in any script 
    public abstract void Run();
    //Called when currentState is changed
    public virtual void Exit() { }
}
