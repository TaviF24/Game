using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    ObjectPool objectPool;
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if(enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            //Shoot();
            if (shotTimer > gunEnemy.secondsBetweenShots)
            {
                Shoot();
            }
            if (moveTimer > Random.Range(2, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer>8)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        objectPool = gameObject.GetComponent<ObjectPool>();
        AudioSource shootSound = gameObject.GetComponent<AudioSource>();
        shootSound.Play();
        //store ref to gun barrel
        Transform gunbarrel = gunEnemy.gunBarrel;
        //new bullet
        GameObject bullet = objectPool.getFreeObject();
        if (bullet != null)
        {
            bullet.transform.position = gunbarrel.position;
            bullet.transform.rotation = enemy.transform.rotation;
            bullet.SetActive(true);
            gunEnemy.muzzleFlash.Play();
            //compute direction
            Vector3 shotDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;
            //add force
            bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shotDirection * 60;
            Debug.Log("Shoot");
            
        }
        shotTimer = 0;
    }
}
