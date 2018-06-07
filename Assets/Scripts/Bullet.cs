using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 35f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameRepository.enemyTag)
        {
            other.GetComponent<Enemy>().DecreaseHealth(damage, false);
        }

        Destroy(gameObject);
    }
}
