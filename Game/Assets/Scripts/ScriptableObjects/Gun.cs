using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public GunData gunData;
    [SerializeField] Transform gunBarrel;
    [SerializeField] Transform imaginaryTarget;
    [SerializeField] GameObject bullet;
    [SerializeField] ObjectPool objectPool;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject mag;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip shotSound;
    [SerializeField] AudioClip reloadSound;

    public bool isInsideWall1 = false;
    public bool isInsideWall2 = false;

    private bool isBlockedFromShooting = false;

    float timeSinceLastShot;
    Camera camera;
    GameObject player;
    Vector3 shotDirection;
    Animator weaponAnimator;
    Animator magAnimator;
    AudioSource audioSource;
    Sound sound = new Sound(Vector3.zero, 20f);

    public LayerMask layerToIgnore;

    private void Start()
    {
        player = GameManager.instance.player;
        camera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
        weaponAnimator = weapon.GetComponent<Animator>();
        magAnimator = mag.GetComponent<Animator>();

        audioSource = gameObject.GetComponent<AudioSource>();
       
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
        if (!gunData.reloading && /*!isInsideWall &&*/ timeSinceLastShot > 1f / (gunData.fireRate / 60f) && !isBlockedFromShooting) 
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
                

                // change the bullet orientation
                Vector3 angles = newBullet.transform.eulerAngles;
                newBullet.transform.eulerAngles = new Vector3(
                    angles.x += camera.transform.eulerAngles.x + 90,
                    angles.y,
                    angles.z
                    );

                Ray ray = new Ray(camera.transform.position, camera.transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * gunData.maxDistance, Color.blue);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, gunData.maxDistance, ~layerToIgnore))
                {
                    //Debug.Log(hitInfo.transform.name);
                    shotDirection = (hitInfo.point - gunBarrel.transform.position).normalized;
                }
                else
                {
                    shotDirection = (imaginaryTarget.transform.position - gunBarrel.transform.position).normalized;
                }

                if (!isInsideWall1 && !isInsideWall2)
                {
                    newBullet.SetActive(true);
                    newBullet.GetComponent<Rigidbody>().velocity = shotDirection * 80;
                }

                muzzleFlash.Play();
                magAnimator.SetTrigger("Shoot");
                weaponAnimator.SetTrigger("Shoot");
                AudioManager.instance.PlayOneShot(shotSound, audioSource);
                sound.position = gunBarrel.position;
                MakeSounds.MakeNoise(sound);
                
                gunData.currentAmo--;
                timeSinceLastShot = 0f;

                if(gunData.currentAmo == 0 && !gunData.reloading)
                {
                    StartCoroutine(Reload());
                }
            }
            
        }

    }

    public void BlockShooting(bool isBlockedFromShootingArg)
    {
        isBlockedFromShooting = isBlockedFromShootingArg;
    }

    public void StartReload()
    {
        if (!gunData.reloading && gunData.currentAmo != gunData.magSize)
        {
            StartCoroutine(Reload());
        }
    }
    
    private IEnumerator Reload()
    {
        gunData.reloading = true;
        weaponAnimator.SetTrigger("Reload");
        magAnimator.SetTrigger("Reload");
        AudioManager.instance.Play(reloadSound);
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmo = gunData.magSize;
        gunData.reloading = false;
    }

}
