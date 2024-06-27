using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour // Singleton
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingSound_M1911;
    public AudioSource reloadingSound_M1911;
    public AudioSource emptyMagazine;


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
}
