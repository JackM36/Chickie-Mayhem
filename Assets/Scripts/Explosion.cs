using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 100f;
    public float explosionRadius = 25.0f;
    public float explosionPower = 200.0f;
    // higher value - higher fly for the affected rigidbodies
    public float explosionLift = 100.0f;
    public float explosionDelay = 0.0f;

    public Transform explosionPrefab;

    Character holder;
    GameObject particles;

    void OnCollisionEnter(Collision collision)
    {
        if (holder == null || collision.transform != holder.transform)
        {
            particles = Instantiate(explosionPrefab, transform.position, transform.rotation).gameObject;
            Vector3 grenadeOrigin = transform.position;
            Collider[] colliders = Physics.OverlapSphere(grenadeOrigin, explosionRadius);

            foreach (Collider c in colliders)
            {
                if (c.transform.GetComponent<Rigidbody>() != null)
                {
                    c.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, grenadeOrigin, explosionRadius, explosionLift);
                    Enemy enemy = c.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        float damage = Mathf.Lerp(0, this.damage, (1 -  Vector3.Distance(enemy.transform.position, transform.position) / explosionRadius));
                        enemy.DecreaseHealth(damage, true);

                        // Set their mats to burned
                        if(enemy.Health == 0)
                        {
                            enemy.SetMaterial(enemy.burnedMat);
                        }
                    }
                }
            }

            Destroy(gameObject);

			AkSoundEngine.PostEvent ("Grenade_Impact", gameObject);

        }
    }

    public void SetHolder(Character holder)
    {
        this.holder = holder;
    }
}
