using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;

    public int damage;


    public void Start()
    {
        zombieHand.damage = this.damage;
    }
}
