using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector2 boundaries;
    public LayerMask groundLayer;
    public float spawnDist = 0.1f;
    public float weaponsSpawnTime = 3f;
    public float powerupsSpawnTime = 6f;
    public int maxSpawnedItems = 20;

    public GameObject[] playerSpawnPoints;
    public Player[] playerCharacters;
    public Weapon[] weapons;
    public Powerup[] powerups;

    float lastPowerupSpawnOnTime = 0;
    float lastWeaponSpawnOnTime = 0;
    int spawneditems = 0;

    private static Spawner _instance = null;
    public static Spawner instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (Spawner)FindObjectOfType(typeof(Spawner));
            }
            return _instance;
        }
    }

    void Update()
    {
        spawneditems = GameObject.FindGameObjectsWithTag(GameRepository.weaponTag).Length + GameObject.FindGameObjectsWithTag(GameRepository.powerupTag).Length;

        SpawnWeapons();
        SpawnPowerups();
    }

    void SpawnPowerups()
    {
        if (Time.time - lastPowerupSpawnOnTime < powerupsSpawnTime || spawneditems >= maxSpawnedItems)
        {
            return;
        }

        // Get point
        float x = Random.Range(transform.position.x - boundaries.x, transform.position.x + boundaries.x);
        float z = Random.Range(transform.position.z - boundaries.y, transform.position.z + boundaries.y);
        Vector3 spawnPoint = new Vector3(x, transform.position.y, z);

        RaycastHit hit;
        bool validSpawnPoint = false;
        int tries = 50;
        do
        {
            if (Physics.Raycast(spawnPoint, Vector3.down, out hit, groundLayer))
            {
                spawnPoint = new Vector3(spawnPoint.x, hit.point.y, spawnPoint.z);
                Debug.DrawLine(transform.position, spawnPoint);
            }

            Collider[] cols = Physics.OverlapSphere(spawnPoint, spawnDist, 1 << groundLayer);
            if (cols.Length == 0)
            {
                validSpawnPoint = true;
            }

            tries--;
        } while (tries > 0 && !validSpawnPoint);

        if (validSpawnPoint)
        {
            // choose what item to spawn
            int itemType = Random.Range(0, 2);
            if (itemType == 0)
            {
                // Spawn weapon
                int itemIndex = Random.Range(0, weapons.Length);
                Instantiate(weapons[itemIndex], spawnPoint, Quaternion.identity);
            }
            else
            {
                // Spawn powerup
                int itemIndex = Random.Range(0, powerups.Length);
                Instantiate(powerups[itemIndex], spawnPoint, Quaternion.identity);
            }

            lastPowerupSpawnOnTime = Time.time;
        }
    }

    void SpawnWeapons()
    {
        if(Time.time - lastWeaponSpawnOnTime < weaponsSpawnTime || spawneditems >=  maxSpawnedItems)
        {
            return;
        }

        // Get point
        float x = Random.Range(transform.position.x - boundaries.x, transform.position.x + boundaries.x);
        float z = Random.Range(transform.position.z - boundaries.y, transform.position.z + boundaries.y);
        Vector3 spawnPoint = new Vector3(x, transform.position.y, z);

        RaycastHit hit;
        bool validSpawnPoint = false;
        int tries = 50;
        do
        {
            if (Physics.Raycast(spawnPoint, Vector3.down, out hit, groundLayer))
            {
                spawnPoint = new Vector3(spawnPoint.x, hit.point.y, spawnPoint.z);
                Debug.DrawLine(transform.position, spawnPoint);
            }

            Collider[] cols = Physics.OverlapSphere(spawnPoint, spawnDist, 1 << groundLayer);
            if(cols.Length == 0)
            {
                validSpawnPoint = true;
            }

            tries--;
        } while (tries > 0 && !validSpawnPoint);

        if (validSpawnPoint)
        {
            // choose what item to spawn
            int itemType = Random.Range(0, 2);
            if(itemType == 0)
            {
                // Spawn weapon
                int itemIndex = Random.Range(0, weapons.Length);
                Instantiate(weapons[itemIndex], spawnPoint, Quaternion.identity);
            }
            else
            {
                // Spawn powerup
                int itemIndex = Random.Range(0, powerups.Length);
                Instantiate(powerups[itemIndex], spawnPoint, Quaternion.identity);
            }

            lastWeaponSpawnOnTime = Time.time;
        }
    }

    public static void SpawnPlayers(int players)
    {
        for(int i = 0; i < players; i++)
        {
            int characterIndex = PlayerPrefs.GetInt("Player" + (i+1) + "Char", 0);
            if(characterIndex == -1)
            {
                continue;
            }
            GameObject player = Instantiate(instance.playerCharacters[characterIndex].gameObject, instance.playerSpawnPoints[i].transform.position, Quaternion.identity);
            player.GetComponent<Player>().playerID = i + 1;

        }

		// AkSoundEngine.PostEvent ("Character_VO", gameObject);
			
    }

    void OnDrawGizmos()
    {
        // Show the grid gizmo
        Gizmos.color = Color. green;
        Gizmos.DrawWireCube(transform.position, new Vector3(boundaries.x * 2, 0.5f, boundaries.y * 2));
    }
}
