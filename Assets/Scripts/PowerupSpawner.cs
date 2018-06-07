using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {
    [SerializeField] List<Powerup> powerupPrefabs;
    bool isSpawningCrate;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        if (!isSpawningCrate)
            StartCoroutine (SpawnCrates (30));
    }

    IEnumerator SpawnCrates(float waitTime){
        isSpawningCrate = true;
		int randomX = Random.Range (-20, 20);
		int randomZ = Random.Range (-20, 20);
		GameObject powerup = (GameObject)Instantiate (powerupPrefabs [RandomNumber ()].gameObject, transform.position + new Vector3(randomX, 20, randomZ), transform.rotation);
     
		yield return new WaitForSeconds (waitTime);
        isSpawningCrate = false;
    }

    public int RandomNumber(){
        int randomNr = Random.Range (0, powerupPrefabs.Count);
        return randomNr;
    }
}
