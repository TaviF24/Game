using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelColliding : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GetComponentInParent<Gun>().isInsideWall1 = true;
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GetComponentInParent<Gun>().isInsideWall1 = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GetComponentInParent<Gun>().isInsideWall1 = false;
    }
}
