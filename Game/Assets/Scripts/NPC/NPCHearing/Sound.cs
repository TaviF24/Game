using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    
    public Vector3 position;
    public float range;
    public Sound(Vector3 _position, float _range)
    {
        position = _position;
        range = _range;
    }

    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(position, range);
    //}
}
