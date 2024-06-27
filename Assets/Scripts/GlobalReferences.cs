using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalReferences : MonoBehaviour // Singleton
{
    public static GlobalReferences Instance { get; set; }

    public GameObject bulletImpactEffectPrefab;


    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
