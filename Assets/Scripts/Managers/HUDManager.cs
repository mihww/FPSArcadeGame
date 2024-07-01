using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using static Weapon;

public class HUDManager : MonoBehaviour // Singleton
{
    public static HUDManager Instance { get; set; }

    [Header("------ Ammo ------")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("------ Weapon ------")]
    public Image activeWeaponUI;
    public Image inactiveWeaponUI;

    [Header("------ Throwables ------")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject crosshair;

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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
        Weapon inactiveWeapon = GetInactiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.weaponModel).ToString();

            Weapon.EWeaponModel model = activeWeapon.weaponModel;

            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (inactiveWeapon)
            {
                inactiveWeaponUI.sprite = GetWeaponSprite(inactiveWeapon.weaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            inactiveWeaponUI.sprite = emptySlot;

        }


        if (WeaponManager.Instance.lethalsCount <= 0)         lethalUI.sprite = greySlot;
        if (WeaponManager.Instance.tacticalsCount <= 0)       tacticalUI.sprite = greySlot;

    }

    public void UpdateThrowablesUI(Throwable.EThrowableType throwableType)
    {
        lethalAmountUI.text = WeaponManager.Instance.lethalsCount.ToString();
        tacticalAmountUI.text = WeaponManager.Instance.tacticalsCount.ToString();

        switch (throwableType)
        {
            case Throwable.EThrowableType.Grenade:
                lethalUI.sprite = IconManager.Grenade;
                break;
            case Throwable.EThrowableType.Smoke_Grenade:
                Debug.Log("WEDIDIT");
                tacticalUI.sprite = IconManager.Smoke_Grenade;
                break;
        }
    }

/* ------------------------------------------------------------------------------------------------------------------------------ */

    #region Getters
private Sprite GetWeaponSprite(EWeaponModel model)
{
    switch (model)
    {
        case EWeaponModel.Pistol1911:
            return IconManager.Pistol1911_Weapon;
        case EWeaponModel.M16:
            return IconManager.M16_Weapon;
        default:
            return null;
    }
}

private Sprite GetAmmoSprite(EWeaponModel model)
{
    switch (model)
    {
        case EWeaponModel.Pistol1911:
            return IconManager.Pistol_Ammo;
        case EWeaponModel.M16:
            return IconManager.Rifle_Ammo;
        default:
            return null;

    }
}

private GameObject GetInactiveWeaponSlot()
{
    foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
    {
        if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
        {
            return weaponSlot;
        }
    }

    return null; // will not happen
}
    #endregion

    private void SetWeaponUI()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
        Weapon inactiveWeapon = GetInactiveWeaponSlot().GetComponentInChildren<Weapon>();
        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.weaponModel).ToString();

            Weapon.EWeaponModel model = activeWeapon.weaponModel;

            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (inactiveWeapon)
            {
                inactiveWeaponUI.sprite = GetWeaponSprite(inactiveWeapon.weaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            inactiveWeaponUI.sprite = emptySlot;
        }
    }

}