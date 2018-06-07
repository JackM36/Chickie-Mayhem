using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Vector3 posOnPlayer;
    public Transform shootPos;
    public bool continuousShooting = false;
    public bool disableRendererOnPickup = false;
    public GameObject projectilePrefab;
    public GameObject muzzleFlashPrefab;
    public float projectileForce;
    public float shootDelay = 0.2f;
    public int ammo = 20;

    Character holder;
    float lastShotOnTime = 0;

    public void SetHolder(Character holder)
    {
        this.holder = holder;

        if(disableRendererOnPickup)
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                rend.enabled = false;
            }
        }

		AkSoundEngine.PostEvent ("Weapon_Pickup", gameObject);
    }

    public void Shoot()
    {
        if (Time.time - lastShotOnTime > shootDelay && ammo > 0)
        {
            // Create the projectile
            Vector3 pos = shootPos.position;
            Quaternion rot = shootPos.rotation;

            // check if weapon has a projectile or not
            if (projectilePrefab != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, pos, rot);
                Rigidbody projectileRb = projectile.AddComponent<Rigidbody>();

                if(muzzleFlashPrefab != null)
                {
                    GameObject muzzle = Instantiate(muzzleFlashPrefab, pos, rot);
                    muzzle.transform.SetParent(transform, false);
                    muzzle.transform.localPosition = shootPos.localPosition;
                    muzzle.transform.forward = shootPos.forward;
                    Destroy(muzzle, 0.3f);

                }

                // check if weapon is a grenade
                if (projectile.GetComponent<Explosion>() != null)
                {
                    projectile.GetComponent<Explosion>().SetHolder(holder);
                }

                // Apply force to projectile to throw it
                projectileRb.AddForce(transform.forward * projectileForce);
            }
            else
            {
                // check if weapon is a laser
                Laser laser = GetComponentInChildren<Laser>();
                if (laser != null)
                {
                    laser.Fire();
                }

                // check if weapon is a flamethrower
                FlameThrower flamethrower = GetComponentInChildren<FlameThrower>();
                if (flamethrower != null)
                {
                    flamethrower.Fire();
                }

            }

            ammo--;
            lastShotOnTime = Time.time;

			AkSoundEngine.PostEvent ("Weapon_Fire", gameObject);

        }
    }

    public void Stop()
    {
        // check if weapon is a flamethrower
        FlameThrower flamethrower = GetComponentInChildren<FlameThrower>();
        if (flamethrower != null)
        {
            flamethrower.StopFiring();
        }
    }
}
