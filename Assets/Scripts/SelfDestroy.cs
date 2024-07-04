using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float timeToDestroy = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf(timeToDestroy));
    }

    private IEnumerator DestroySelf(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
