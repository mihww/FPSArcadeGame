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

    [Header("----- Throwables General -----")]
    public float throwForce = 10f;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;

    [Header("----- Lethals -----")]
    public int maxLethals = 2;
    public int lethalsCount = 0;
    public Throwable.EThrowableType equippedLethalType;
    public GameObject grenadePrefab;


    [Header("----- Tacticals -----")]
    public int maxTacticals = 2;
    public int tacticalsCount = 0;
    public Throwable.EThrowableType equippedTacticalType;
    public GameObject smokeGrenadePrefab;



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

        if (activeWeaponSlot.transform.childCount > 0)         InvokeWeaponChanged(activeWeaponSlot.transform.GetChild(0).gameObject);

        equippedLethalType = Throwable.EThrowableType.None;
        equippedTacticalType = Throwable.EThrowableType.None;
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

        switch (true)
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
                if (lethalsCount > 0)
                {
                    ThrowLethal();
                }

                forceMultiplier = 0;
                break;

            /* rest of lethals */

            // Tacticals
            case bool _ when Input.GetKeyUp(KeyCode.T):
                if (tacticalsCount > 0)
                {
                    ThrowTactical();
                }

                forceMultiplier = 0;
                break;





            // Others
            case bool _ when (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T)):

                forceMultiplier += Time.deltaTime;

                if (forceMultiplier > forceMultiplierLimit)
                {
                    forceMultiplier = forceMultiplierLimit;
                }
                break;


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
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        // Activate the new weapon
        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
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
                PickupThrowableAsLethal(Throwable.EThrowableType.Grenade);
                break;
            case Throwable.EThrowableType.Smoke_Grenade:
                PickupThrowableAsTactical(Throwable.EThrowableType.Smoke_Grenade);
                break;
        }

    }

    private void PickupThrowableAsTactical(Throwable.EThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.EThrowableType.None)
        {
            equippedTacticalType = tactical;

            if (tacticalsCount < maxTacticals)
            {
                tacticalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowablesUI(equippedTacticalType);
            }
            else
            {
                print("tacticals limit reached");
            }
        }
        else
        {
            // Cannot pick up tacticals
            // Switch tacticals
        }
    }

    private void PickupThrowableAsLethal(Throwable.EThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.EThrowableType.None)
        {
            equippedLethalType = lethal;

            if (lethalsCount < maxLethals)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowablesUI(equippedLethalType);
            }
            else
            {
                print("lethals limit reached");
            }
        }
        else
        {
            // Cannot pick up lethal
            // Switch lethal
        }
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
    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            // Settnig original scale
            var originalScale = weaponToDrop.transform.localScale;

            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
            
            // Storing to the scale - it should fix the scale issue due to the parent change
            weaponToDrop.transform.localScale = originalScale;
        }
    }

    #endregion

    #region || ----- Handling Throwables methods ----- ||

    private GameObject GetThrowablePrefab(Throwable.EThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.EThrowableType.Grenade:
                return grenadePrefab;
            case Throwable.EThrowableType.Smoke_Grenade:
                return smokeGrenadePrefab;
        }

        return null; // since it will never run
    }
    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);

        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        tacticalsCount--;

        if (tacticalsCount <= 0)
        {
            equippedTacticalType = Throwable.EThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowablesUI(equippedTacticalType);
    }
    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        lethalsCount--;

        if (lethalsCount <= 0)
        {
            equippedLethalType = Throwable.EThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowablesUI(equippedLethalType);
    }


    #endregion

}
