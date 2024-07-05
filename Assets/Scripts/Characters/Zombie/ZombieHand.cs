using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHand : MonoBehaviour
{
    public int damage;


    private void Update()
    {
        Animator animator = gameObject.GetComponentInParent<Animator>();
        if (animator == null)     return;
        if (gameObject.GetComponentInParent<Enemy>().isDead == true)
        {
            gameObject.SetActive(false);
        }
    }
}
