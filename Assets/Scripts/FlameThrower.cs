using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float radius = 4f;
    public float dist = 10f;
    public float damage = 1;

    ParticleSystem flames;
    ParticleSystem.EmissionModule emmision;

    

    void Start()
    {
        flames = GetComponentInChildren<ParticleSystem>();
        emmision = flames.emission;
    }

    public void Fire()
    {
        emmision.enabled = true;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.forward, dist, enemyLayer);
        foreach(RaycastHit hit in hits)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            enemy.DecreaseHealth(damage, true);

            // Set their mats to burned
            if (enemy.Health == 0)
            {
                enemy.SetMaterial(enemy.burnedMat);
            }
        }
    }

    public void StopFiring()
    {
        emmision.enabled = false;
    }


}
