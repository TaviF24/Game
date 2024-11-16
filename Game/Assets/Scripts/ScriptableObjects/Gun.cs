using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;
    [SerializeField] Transform gunBarrel;
    [SerializeField] Transform imaginaryTarget;
    [SerializeField] GameObject bullet;
    [SerializeField] ObjectPool objectPool;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject mag;

    float timeSinceLastShot;
    Camera camera;
    GameObject player;
    Vector3 shotDirection;
   
    private void Start()
    {
        player = GameManager.instance.player;
        camera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
        if (gunData.reloading)
        {
            gunData.reloading = false;
            
        }
        if(gunData.currentAmo == 0)
        {
            StartReload();
        }
        
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        //Debug.Log(camera.transform.rotation.eulerAngles);
        //Debug.Log(DateTime.Now);
    }

    private bool CanShoot()
    {   
        //fireRate = 600 per min => 600/60 = 10 per sec => 1/10 = 0.1 s between bullets
        if (!gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f)) 
        {
            return true;
        }
        return false;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            GameObject newBullet = objectPool.getFreeObject();


            //GameObject newBullet = GameObject.Instantiate(bullet, gunBarrel.position, player.transform.rotation);

            if(newBullet != null) 
            {
                newBullet.transform.position = gunBarrel.position;
                newBullet.transform.rotation = player.transform.rotation;
                newBullet.SetActive(true);

                // change the bullet orientation
                Vector3 angles = newBullet.transform.eulerAngles;
                newBullet.transform.eulerAngles = new Vector3(
                    angles.x += camera.transform.eulerAngles.x + 90,
                    angles.y,
                    angles.z
                    );

                Ray ray = new Ray(camera.transform.position, camera.transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * gunData.maxDistance, Color.blue);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    //Debug.Log(hitInfo.transform.name);
                    shotDirection = (hitInfo.point - gunBarrel.transform.position).normalized;
                }
                else
                {
                    shotDirection = (imaginaryTarget.transform.position - gunBarrel.transform.position).normalized;
                }

                newBullet.GetComponent<Rigidbody>().velocity = shotDirection * 80;

                gunData.currentAmo--;
                timeSinceLastShot = 0f;

                if(gunData.currentAmo == 0)
                {
                    StartReload();
                }
            }
            
        }

    }

    public void StartReload()
    {
        if (!gunData.reloading && gunData.currentAmo != gunData.magSize)
        {
            StartCoroutine(Reload());
            // weapon.GetComponent<Animator>().SetTrigger("Reload");
            // mag.GetComponent<Animator>().SetTrigger("Reload");
        }
    }
    
    private IEnumerator Reload()
    {
        gunData.reloading = true;
        Debug.Log("is reloading...");
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmo = gunData.magSize;
        gunData.reloading = false;
    }

}
