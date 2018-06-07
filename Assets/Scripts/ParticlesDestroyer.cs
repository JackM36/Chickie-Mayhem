using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDestroyer : MonoBehaviour
{
    ParticleSystem particles;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particles != null && !particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
