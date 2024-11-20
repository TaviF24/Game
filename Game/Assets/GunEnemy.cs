using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float secondsBetweenShots;
}
