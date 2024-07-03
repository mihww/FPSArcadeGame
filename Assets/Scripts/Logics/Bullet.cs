using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int bulletDamage;


    private void OnCollisionEnter(Collision collision)
    {
        switch (true)
        {
            case bool _ when collision.gameObject.CompareTag("Target"):
                print(collision.gameObject.name + " was hit by a bullet!");
                CreateBulletImpactEffect(collision);
                Destroy(gameObject);
                break;
            case bool _ when collision.gameObject.CompareTag("Wall"):
                print("Hit a Wall");
                CreateBulletImpactEffect(collision);
                Destroy(gameObject);
                break;
            case bool _ when collision.gameObject.CompareTag("Bottle"):
                print("Hit a Bottle");
                CreateBulletImpactEffect(collision);
                collision.gameObject.GetComponent<Bottle>().Explode();
                break;
            case bool _ when collision.gameObject.CompareTag("Enemy"):
                print("Hit an Enemy");
                collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
                Destroy(gameObject);
                break;
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
