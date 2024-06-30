using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("----- Ammo -----")]
    public int totalPistolAmmo = 0;
    public int totalRifleAmmo = 0;

    [Header("----- Throwables -----")]
    public int grenades = 0;
    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;

    public event Action<GameObject> OnWeaponChanged; // event to notify the weapon change - couldn't find a better way to constantly update the ammo display



    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
        InvokeWeaponChanged(activeWeaponSlot.transform.GetChild(0).gameObject);
    }
    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        switch(true)
        {
            // Weapon Slots
            case bool _ when (Input.GetKeyDown(KeyCode.Alpha1) && Weapon.isShooting == false):
                SwitchActiveSlot(0);
                break;
            case bool _ when (Input.GetKeyDown(KeyCode.Alpha2) && Weapon.isShooting == false):
                SwitchActiveSlot(1);
                break;
            
            // Lethals
            case bool _ when Input.GetKeyUp(KeyCode.G):
                if (grenades > 0)
                {
                    ThrowLethal();
                }

                forceMultiplier = 0;
                break;


            /* rest of lethals */



            // Others
            case bool _ when Input.GetKey(KeyCode.G):
                forceMultiplier += Time.deltaTime;
                if(forceMultiplier > forceMultiplierLimit)
                {
                    forceMultiplier = forceMultiplierLimit;
                }
                break;
                

        }
    }



    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
     if(activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
            
        }
    }
    public void SwitchActiveSlot(int slotNumber)
    {
        // check if the slot number is within the range of the weapon slots
        if (slotNumber < 0 || slotNumber >= weaponSlots.Count)
        {
            Debug.LogError("Slot number out of range.");
            return;
        }

        // Deactivate the current weapon
        if (activeWeaponSlot.transform.childCount>0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        // Activate the new weapon
        activeWeaponSlot = weaponSlots[slotNumber];

        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            if (newWeapon != null)
            {
                newWeapon.isActiveWeapon = true;
                InvokeWeaponChanged(newWeapon.gameObject);
            }
        }

    }


    /* --------------------------------------------------------------------------------------------------- */

    #region || ----- Handling Pickups ----- ||

    #region [Weapon]
    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
        InvokeWeaponChanged(pickedUpWeapon);
    }
    #endregion
    #region [Ammo]
    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.EAmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.EAmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }
    #endregion
    #region [Throwables]
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.EThrowableType.Grenade:
                PickupGrenade();
                break;
        }

    }

    private void PickupGrenade()
    {
        grenades += 1;
        HUDManager.Instance.UpdateThrowables(Throwable.EThrowableType.Grenade);
    }
    #endregion

    #endregion

    #region || ----- Handling Weapon methods ----- ||
    private void InvokeWeaponChanged(GameObject weapon)
    {
        OnWeaponChanged?.Invoke(weapon);
    }
    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);
        TransferWeaponAttributes(pickedUpWeapon);
    }
    private void TransferWeaponAttributes(GameObject pickedUpWeapon)
    {
        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();
        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }
    internal void DecreaseTotalAmmo(int bulletsLeft, Weapon.EWeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.EWeaponModel.Pistol1911:
                totalPistolAmmo -= bulletsLeft;
                break;
            case Weapon.EWeaponModel.M16:
                totalRifleAmmo -= bulletsLeft;
                break;
        }
    }
    public int CheckAmmoLeftFor(EWeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case EWeaponModel.Pistol1911:
                return WeaponManager.Instance.totalPistolAmmo;
            case EWeaponModel.M16:
                return WeaponManager.Instance.totalRifleAmmo;
            default:
                return 0;
        }
    }
    #endregion

    #region || ----- Handling Throwables methods ----- ||
    private void ThrowLethal()
    {
        GameObject lethalPrefab = grenadePrefab;

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        grenades--;
        HUDManager.Instance.UpdateThrowables(Throwable.EThrowableType.Grenade);


    }
    #endregion

}
