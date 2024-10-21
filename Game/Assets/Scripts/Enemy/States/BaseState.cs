using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public Enemy enemy;
    public StateMachine stateMachine;
    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}
