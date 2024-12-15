using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    public GameObject weapon;
    public Animator npcAnim;
    // Start is called before the first frame update
    void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    public void Patrol()
    {
        npcAnim.SetBool("patrolling", true);
    }

    public void StopPatrolling()
    {
        npcAnim.SetBool("patrolling", false);
    }

    public void PatrolWithWeapon()
    {
        npcAnim.SetBool("too_far", !npcAnim.GetBool("too_far"));
    }

    public void StopPatrollingWithWeapon()
    { // Same as stop patrolling with weapon
        npcAnim.SetTrigger("detected");
        npcAnim.SetBool("still_detecting", true);
        npcAnim.SetBool("too_far", false);
    }

    public void PutDownWeapon()
    {
        npcAnim.SetBool("still_detecting", false);
    }
}
