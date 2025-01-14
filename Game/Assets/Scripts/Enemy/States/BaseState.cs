using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public Enemy enemy;
    public GunEnemy gunEnemy;
    public StateMachine stateMachine;

    public GameObject gameObject;

    public void SetGameObject(GameObject go)
    {
        gameObject = go;
    }

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}
