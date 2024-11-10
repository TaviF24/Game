using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GunData weaponThatIsUsingMe;

    Vector3 prevPos;

    private void Start()
    {
        prevPos = transform.position;
    }

    private void Update()
    {

        Vector3 direction = (transform.position - prevPos).normalized;
        float distance = (transform.position - prevPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(prevPos, direction, out hit, distance))
        {
            IDamageable component = hit.collider.gameObject.GetComponent<IDamageable>();
            if (component != null)
            {
                component.TakeDamage(weaponThatIsUsingMe.damage);
            }
            gameObject.SetActive(false);
        }
        
        /*  //This can be used if we want to shoot through different layers (doors, windows, walls, etc.)
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, direction), distance);

        for (int i = 0; i < hits.Length; i++) 
        {
            Debug.Log (hits.Length + " loveste " + hits[i].collider.gameObject.name);
        }
        */

        prevPos = transform.position;
    }
}
