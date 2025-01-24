using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    
    public BaseState activeState = new PatrolState();
   

    public void Initialize()
    {
        
        ChangeState(new PatrolState());
    }

    // Start is called before the first frame update
    void Start()
    {
        //activeState.SetGameObject(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        // change to new state
        activeState = newState;

        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.gunEnemy = GetComponentInChildren<GunEnemy>();
            activeState.SetGameObject(gameObject);
            activeState.Enter();
        }
    }
}
