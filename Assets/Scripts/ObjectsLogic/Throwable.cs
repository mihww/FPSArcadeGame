using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 50f;
    [SerializeField] float explosionForce = 1200f;

    private float countdown;
    private bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum EThrowableType
    {
        Grenade
    }
    public EThrowableType throwableType;


    void Start()
    {
        countdown = delay;
    }
    void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }


    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case EThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        // Visual effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffectPrefab;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Physical effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            // will add enemies here


        }
    }
}
