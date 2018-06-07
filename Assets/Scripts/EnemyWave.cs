using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    Transform spawnPointsContainer;
	[SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyBigPrefab;
    bool enemyIsSpawning;
	int amountOfEnemiesSpawned;
	bool allEnemiesSpawned;

    public Transform[] spawnPoints;
	// Set by the EnemyWaveSpawner class
	public int spawnLimit;

	// Use this for initialization
	void Awake()
    {
        spawnPointsContainer = GameObject.FindGameObjectWithTag(GameRepository.enemiesSpawnPointsTag).transform;
        Transform[] spawnPointsTemp = spawnPointsContainer.GetComponentsInChildren<Transform>();
        spawnPoints = new Transform[spawnPointsTemp.Length - 1];
        int j = 0;
        for(int i = 0; i < spawnPointsTemp.Length; i++)
        {
            if(spawnPointsTemp[i] != spawnPointsContainer)
            {
                spawnPoints[j] = spawnPointsTemp[i];
                j++;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (!enemyIsSpawning) {
			StartCoroutine (CreateEnemyWave (2));
		}

		// Check if all enemies of the wave are dead
		if (transform.childCount <= 0 && allEnemiesSpawned) {
			Destroy (gameObject);
		}
	}

	void SpawnNewEnemy(){
		if (enemyPrefab != null && amountOfEnemiesSpawned < spawnLimit) {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
			GameObject enemy = Instantiate (enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity) as GameObject;
			enemy.transform.parent = transform;

			AkSoundEngine.PostEvent("Chickie_Spawn", gameObject);
			AkSoundEngine.PostEvent("Chickie_Cluck", gameObject);
		}
	}

	IEnumerator CreateEnemyWave(float waitTime){
		enemyIsSpawning = true;
		SpawnNewEnemy ();
		amountOfEnemiesSpawned++;
		CheckAllEnemiesSpawned ();
		yield return new WaitForSeconds (waitTime);
		enemyIsSpawning = false;
	}

	// Check if all the enemies of the wave are spawned
	void CheckAllEnemiesSpawned(){
		if (amountOfEnemiesSpawned == spawnLimit) {
			allEnemiesSpawned = true;
		}
	}
}
