using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;
    [SerializeField] Transform gunBarrel;
    [SerializeField] Transform imaginaryTarget;
    [SerializeField] GameObject bullet;

    float timeSinceLastShot;
    Camera camera;
    GameObject player;
   
    
    private void Awake()
    {
        player = GameManager.Instance.player;
        camera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        //Debug.Log(camera.transform.rotation.eulerAngles);
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
            GameObject newBullet = GameObject.Instantiate(bullet, gunBarrel.position, player.transform.rotation);
            
            // change the bullet orientation
            Vector3 angles = newBullet.transform.eulerAngles;
            newBullet.transform.eulerAngles = new Vector3 (
                angles.x += camera.transform.eulerAngles.x + 90,
                angles.y,
                angles.z
                );

            Vector3 shotDirection = (imaginaryTarget.transform.position - player.transform.position).normalized;

            newBullet.GetComponent<Rigidbody>().velocity = shotDirection * 20;            
            


            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
            {
                //Debug.Log(hitInfo.transform.name);
            }
            gunData.currentAmo--;
            timeSinceLastShot=0f;
        }

    }

}
