using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    public GameObject weapon;
    Animator npcAnim;
    // Start is called before the first frame update
    void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            npcAnim.SetBool("patrolling",true);
            print("patrolling");
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            npcAnim.SetTrigger("turn_right");
            npcAnim.SetBool("patrolling",false);
            print("turning right");
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            npcAnim.SetBool("patrolling",false);
            print("stop");
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            npcAnim.SetTrigger("detected");
            npcAnim.SetBool("still_detecting",true);
            npcAnim.SetBool("too_far",false);
            print("detected");
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            npcAnim.SetBool("too_far",!npcAnim.GetBool("too_far"));
            print("too_far "+npcAnim.GetBool("too_far"));
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            npcAnim.SetBool("still_detecting",false);
            print("stop detected");
        }
    }
}
