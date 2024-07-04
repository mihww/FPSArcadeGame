using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private NavMeshAgent navAgent;
    private Animator animator;
    private bool isDead = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;
        HP -= damageAmount;
        if (HP <= 0)
        {
            isDead = true;
            int randomValue = Random.Range(0, 2);

            switch (randomValue)
            {
                case 0:
                    animator.SetTrigger("DIE1");
                    break;
                case 1:
                    animator.SetTrigger("DIE2");
                    break;

            }
            // Dead Sound
            SoundManager.Instance.zombiesChannel.PlayOneShot(SoundManager.Instance.zombieDeath);                  // hardcoded this bc im lazy

        }
        else
        {
            animator.SetTrigger("DAMAGE");
            // Damage Sound
            SoundManager.Instance.zombiesChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);                  // hardcoded this bc im lazy

        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f); // attacking + stop attacking distance

        Gizmos.color= Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // detection area radius

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 25f); // stop chasing distance
    }
#endif

}
