using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    #region Variables

    public EWeaponModel weaponModel;
    public bool isActiveWeapon;

    #region Shooting Variables
    [Header("---------- Shooting details ----------")]
    public static bool isShooting;
    public bool readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;
    public EShootingMode currentShootingMode;
    #endregion

    #region Enums
    public enum EShootingMode
    {
        Single,
        Burst,
        Auto
    }


    public enum EWeaponModel
    {
        Pistol1911,
        M16
    }
    #endregion

    #region Burst Variables
    public uint bulletsPerBurst = 3;
    public uint burstBulletsLeft;
    #endregion

    #region Spread Variable
    public float spreadIntensity;
    #endregion

    #region Bullet variables
    [Header("---------- Bullet miscs ----------")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;
    #endregion

    #region Miscs
    [Header("---------- Miscs ----------")]
    public GameObject muzzleEffect;
    internal Animator animator;
    #endregion

    #region Reloading Variables
    [Header("---------- Reload details ----------")]
    public float reloadTime;
    public uint magazineSize;
    public uint bulletsLeft;
    public bool isReloading;
    #endregion

    #region Positions
    [Header("---------- Weapon Positions ----------")]
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    #endregion

    #endregion

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;

    }


    void Update()
    {
        if(isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;    
          
        if (bulletsLeft == 0 && isShooting)
        {
            // This will stay hardcoded since all weapons have the same empty magazine sound
            SoundManager.Instance.emptyMagazine.Play();
        }

        #region Handling Keys
        if (currentShootingMode == EShootingMode.Auto)
        {
            // Holding down Mouse0
            isShooting = Input.GetKey(KeyCode.Mouse0) && !isReloading;
        }
        else if (currentShootingMode == EShootingMode.Single ||
                currentShootingMode == EShootingMode.Burst)
        {
            // Clicking Mouse0 Once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0) && !isReloading;
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {

            Reload();

        }

        // Auto reload when magazine is empty
        if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
        {
            Reload();
        }
        }
        #endregion

        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
            Debug.Log("Attempting to fire bullet.");
        }



    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadingSound(weaponModel);

        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke(nameof(ReloadCompleted), reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
        
    }

    private void FireWeapon()
    {
        if (isActiveWeapon)
        {
            bulletsLeft--;

            muzzleEffect.GetComponent<ParticleSystem>().Play();
            animator.SetTrigger("RECOIL");

            SoundManager.Instance.PlayShootingSound(weaponModel);

            readyToShoot = false;

            Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


            // Instatiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // Pointing the bullet to face the shooting direction        
            bullet.transform.forward = shootingDirection;

            // Shoot the bullet
            bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse); // fw - blue axis
            Debug.Log("Bullet instantiated.");

            // Destroy the bullet   
            StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

            // Check if shooting is done
            if (allowReset)
            {

                Invoke(nameof(ResetShot), shootingDelay);
                allowReset = false;
            }

            // Burst Mode
            if (currentShootingMode == EShootingMode.Burst && burstBulletsLeft > 1) // alr shoot once before this check
            {
                burstBulletsLeft--;

                Invoke(nameof(FireWeapon), shootingDelay);

                Debug.Log("Attempting to fire bullet.");

            }
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the middle of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            // Hitting smth
            targetPoint = hit.point;
        }
        else
        {
            // Shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);


        // Return the direction with the spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(bullet);
    }
}
