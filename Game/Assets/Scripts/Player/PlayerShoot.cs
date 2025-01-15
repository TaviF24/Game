using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] public Gun gun;
    [SerializeField] public TextMeshProUGUI viewAmmoCount;

    public void Shoot()
    {
        viewAmmoCount.text = gun.gunData.currentAmo.ToString();
        gun.Shoot();
    }

    public void Reload()
    {
        gun.StartReload();
        viewAmmoCount.text = gun.gunData.magSize.ToString();
    }

    public void BlockShooting(bool isBlockedFromShootingArg)
    {
        gun.BlockShooting(isBlockedFromShootingArg);
    }
}
