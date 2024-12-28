using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Gun gun;

    public void Shoot()
    {
        gun.Shoot();
    }

    public void Reload()
    {
        gun.StartReload();
    }

    public void BlockShooting(bool isBlockedFromShootingArg)
    {
        gun.BlockShooting(isBlockedFromShootingArg);
    }
}
