using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
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
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }

    private void Update()
    {
        if (navAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("IS_WALKING", true);
        }
        else
        {
            animator.SetBool("IS_WALKING", false);
        }
    }
}
