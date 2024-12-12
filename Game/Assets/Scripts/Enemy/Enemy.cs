using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHear
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;

    public GameObject debugsphere;
    private Vector3 lastKnownPos;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }
    
    public PathAI path;
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [SerializeField]
    private string currentState; // debugging

    [HideInInspector]
    public NPCAnim anim;
    public bool alreadyStartedAnim = false;
    

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<NPCAnim>();
        stateMachine.Initialize();
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity != Vector3.zero) 
        {
            if (anim.npcAnim.GetBool("still_detecting")) 
            {
                if (!alreadyStartedAnim)
                {
                    anim.PatrolWithWeapon();
                    alreadyStartedAnim = true;
                }
            }
            else
            {
                anim.Patrol();
            }
        }
        else
        {
            if (anim.npcAnim.GetBool("still_detecting"))
            {
                anim.StopPatrollingWithWeapon();
                alreadyStartedAnim = false;
            }
            else
            {
                anim.StopPatrolling();
            }
        }

        CanSeePlayer();
        currentState=stateMachine.activeState.ToString();
        debugsphere.transform.position = lastKnownPos;
    }

    public bool CanSeePlayer()
    {
        if(player!=null)
        {
            if(Vector3.Distance(transform.position, player.transform.position) < sightDistance) 
            {
                Vector3 targetDirection = player.transform.position - transform.position- (Vector3.up*eyeHeight);
                float angleToPlayer=Vector3.Angle(targetDirection, transform.forward);
                if(angleToPlayer>=-fieldOfView && angleToPlayer<=fieldOfView) 
                {
                    Ray ray = new Ray(transform.position+ (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo,sightDistance)) 
                    {
                        if(hitInfo.transform.gameObject==player) 
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                    
                }
            }
        }
        return false;
    }

    public void RespondToSound(Sound sound)
    {
        lastKnownPos = sound.position;
        if(currentState != "AttackState")
        {
            //change to Attacktate if you want the npc to move a little slower and to not come instantly to the sound source
            stateMachine.ChangeState(new SearchState());
        }
    }
}
