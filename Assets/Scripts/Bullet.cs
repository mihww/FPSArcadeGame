using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            print(collision.gameObject.name + " was hit by a bullet!");

            CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("Hit a Wall");

            CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bottle"))
        {
            print("Hit a Bottle");

            CreateBulletImpactEffect(collision);

            collision.gameObject.GetComponent<Bottle>().Explode();
        }
    }

    private void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(objectWeHit.gameObject.transform); 
    }


}
