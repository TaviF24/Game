using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    new private float searchTimer;
    private float moveTimer;

    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
    }

    public override void Perform()
    {
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        if(enemy.Agent.remainingDistance<enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(2, 4))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }
            if (searchTimer > 5) 
            {
                enemy.anim.PutDownWeapon();
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }
    public override void Exit()
    {

    }

}
