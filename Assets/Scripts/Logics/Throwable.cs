using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 50f;
    [SerializeField] private float explosionForce = 1200f;


    private float countdown;
    private bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum EThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade,
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

/* ---------------------------------------------------------------------------------------------------- */

    #region Effects
    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case EThrowableType.Grenade:
                GrenadeEffect();
                break;
            case EThrowableType.Smoke_Grenade:
                SmokeGrenadeEFfect();
                break;
        }
    }

    private void SmokeGrenadeEFfect()
    {
        // Audio Effect
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        // Visual effect
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffectPrefab;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        // Physical effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // smoke
            }



            // will add enemies here
        }
    }

    private void GrenadeEffect()
    {

        // Audio Effect
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

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
    #endregion

}

