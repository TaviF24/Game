using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Gun gun;
    

    public void Shoot()
    {
        AudioSource shootSound = gameObject.GetComponent<AudioSource>();
        shootSound.Play();
        gun.Shoot();
    }

    public void Reload()
    {
        gun.StartReload();
    }
}
