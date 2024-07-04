using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;


    public void TakeDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            print("Player is dead");

            // Game Over
            // Respawn player
            // Dying anim


        }
        else
        {

            print("Player Hit");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ZombieHand"))
        {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
