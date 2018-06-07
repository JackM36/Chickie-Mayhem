using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Wave score handling
	private static string score;
	[SerializeField] Transform scorePrefab;
	static bool shouldChangeScore;
    public float reloadTime = 3;

	[SerializeField] GameObject playerPrefab;

    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Spawn players
        int players = PlayerPrefs.GetInt("Players", 1); ;
        Spawner.SpawnPlayers(players);
    }

	void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (GetPlayers().Count == 0)
        {
            StartCoroutine(Reload(reloadTime));
        }

        if (shouldChangeScore) {
			scorePrefab.GetComponent<Text> ().text = score;
			shouldChangeScore = false;
		}
	}

    public static List<Player> GetPlayers()
    {
        GameObject[] playersObjs = GameObject.FindGameObjectsWithTag(GameRepository.playerTag);
        List<Player> players = new List<Player>();
        foreach (GameObject playerObj in playersObjs)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player.isAlive)
            {
                players.Add(player);
            }
        }

        return players;
    }

	public static void UpdateScore(int roundNr){
		score = roundNr.ToString();
		shouldChangeScore = true;
	}

    IEnumerator Reload(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("PlayerSelectionScene");

		AkSoundEngine.PostEvent ("Chickie_Cluck_Stop", gameObject);

    }
}
