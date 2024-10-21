using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public GameObject player;
    public float AllowDistance = 5;
    public float speed = 1f;
    private RaycastHit shot;

    void Update()
    {
        transform.LookAt(player.transform);
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, Color.red);
        if (Physics.Raycast(transform.position, directionToPlayer, out shot, distanceToPlayer))
        {
            Debug.Log("Raycast hit: " + shot.collider.name + " at distance: " + shot.distance);

            if (shot.distance >= AllowDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                Debug.Log("Moving towards player");
            }
            else
            {
                Debug.Log("Player is within allowed distance, stopping movement");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }
    }
}