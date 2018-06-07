using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour {
	[SerializeField] GameObject EnemyWavePrefab;
	[SerializeField] GameObject BossPrefab;
	GameObject enemyWave;
	bool isSpawningWave;
	int waveNr;
	[SerializeField] int numberOfEnemies = 7;

    int wavesToSpawnBigEnemy = 2;
    int wavesSpawnedWithoutBigEnemy = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!isSpawningWave && enemyWave == null) {
			StartCoroutine(SpawnNewWave (5));

			AkSoundEngine.PostEvent ("Colonel_VO", gameObject);

			if (waveNr > 0 && waveNr % 2 == 0)
				Instantiate (BossPrefab, BossPrefab.transform.position, BossPrefab.transform.rotation);
		}
	}

	// Spawn new wave of enemies
	IEnumerator SpawnNewWave(float waitTime){
		isSpawningWave = true;
		yield return new WaitForSeconds (waitTime);
		enemyWave = Instantiate (EnemyWavePrefab) as GameObject;
		waveNr += 1;
       // wavesSpawnedWithoutBigEnemy++;

        GameManager.UpdateScore (waveNr);
		SetWaveSpawnLimit (numberOfEnemies);
		isSpawningWave = false;
	}

	// Amount of enemies that can be spawned depending of the wave number;
	void SetWaveSpawnLimit(int amountOfEnemies){
		enemyWave.GetComponent<EnemyWave> ().spawnLimit = amountOfEnemies * waveNr;
	}
}
