using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    LineRenderer line;
    public int length = 5;
    public float damage = 10f;
    
	void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
	}

    public void Fire()
    {
        StartCoroutine(LaserFire());
    }

    IEnumerator LaserFire()
    {
        line.enabled = true;

        while (Input.GetKey("joystick " + 1 + " button 7")) // needs to be changed for P2 as well
        {
            line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            line.SetPosition(0, ray.origin);

            if (Physics.Raycast(ray, out hit, length))
            {
                line.SetPosition(1, hit.point);
                if (hit.rigidbody && hit.transform.tag.Equals(GameRepository.enemyTag))
                {
                    hit.transform.GetComponent<Enemy>().DecreaseHealth(damage, false);
                }
            }
            else
            {
                line.SetPosition(1, ray.GetPoint(length));
            }

            yield return null;
        }

        line.enabled = false;
    }
}
