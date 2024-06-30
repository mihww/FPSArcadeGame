using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour // Singleton
{
    public static SoundManager Instance { get; set; }


    [Header("---------- Channels ----------")]
    public AudioSource shootingChannel;
    public AudioSource reloadingChannel;
    public AudioSource emptyMagazine;

    [Header("---------- Pistol M1911 ----------")]
    public AudioClip M1911Shot;
    public AudioClip M1911Reload;
    
    [Header("---------- M16 ----------")]
    public AudioClip M16Shot;
    public AudioClip M16Reload;


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

    public void PlayShootingSound(EWeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case EWeaponModel.Pistol1911:
                shootingChannel.PlayOneShot(M1911Shot);
                break;
            case EWeaponModel.M16:
                shootingChannel.PlayOneShot(M16Shot);
                break;
        }
    }

    public void PlayReloadingSound(EWeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case EWeaponModel.Pistol1911:
                reloadingChannel.PlayOneShot(M1911Reload);
                break;
            case EWeaponModel.M16:
                reloadingChannel.PlayOneShot(M16Reload);
                break;
        }
    }
}
