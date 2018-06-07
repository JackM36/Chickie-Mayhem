using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float attackDist = 1.5f;
    public float attackDelay = 0.5f;

    public Material burnedMat;
    public GameObject bloodPrefab;

    Transform target;
    UnityEngine.AI.NavMeshAgent nav;
    float lastAttackOnTime = 0;

    protected override void Awake()
    {
        base.Awake();

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isAlive)
        {
            // go to closest target
            target = GetTarget();

            if(target != null)
            {
                nav.SetDestination(target.position);
                float distFromTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distFromTarget <= attackDist)
                {
                    Attack();
                }
            }
        }
        else
        {
            nav.velocity = Vector3.zero;
            nav.Stop();
        }

        // Set anim vars
        anim.SetFloat("Speed", currentSpeed);
        anim.SetFloat("Speed01", currentSpeed / nav.speed);
    }

    Transform GetTarget()
    {
        float minDist = float.MaxValue;
        int closestPlayerIndex = 0;
        List<Player> players = GameManager.GetPlayers();

        if(players.Count == 0)
        {
            return null;
        }

        for (int i = 0; i < players.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, players[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestPlayerIndex = i;
            }
        }

        return players[closestPlayerIndex].transform;
    }


	void OnTriggerEnter(Collider col)
    {
		if (col.tag == GameRepository.bulletTag)
        {
            GameObject bloodParticles = Instantiate(bloodPrefab, transform.position, transform.rotation);
            bloodParticles.transform.forward = col.transform.forward;

            Debug.DrawRay(transform.position, col.transform.forward * 20, Color.red, 10);
		}
	}

    void Attack()
    {
        if (Time.time - lastAttackOnTime > attackDelay)
        {
			AkSoundEngine.PostEvent ("Chickie_Attack", gameObject);
        }
    }

    public void SetMaterial(Material mat)
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in rends)
        {
            Material currentMat = rend.material;
            currentMat = mat;
            rend.material = mat;
        }
    }

}
