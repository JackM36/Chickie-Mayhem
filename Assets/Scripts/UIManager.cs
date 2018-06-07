using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [HideInInspector]
    public int characterIDP1, characterIDP2;

    public Player[] playerCharacters;

    // boolean variables to verify each player selection
    [HideInInspector]
    public bool selectedP1, selectedP2;
    [HideInInspector]
    public bool playersJoined;

    // countdown timer to load main scene
    float timer = 4.0f;

    public Text indicatorMessage;

    void Awake()
    {
        selectedP1 = true;
        selectedP2 = true;
        playersJoined = false;
        characterIDP1 = -1;
        characterIDP2 = -1;
    }

    void Update ()
    {
        if (playersJoined && selectedP1 && selectedP2)
        {
            timer -= Time.deltaTime;
            indicatorMessage.text = "Starting in " + (int)timer + "...";
            if (timer < 0.01)
            {
                PlayerPrefs.SetInt("Players", 2);
                PlayerPrefs.SetInt("Player1Char" , characterIDP1);
                PlayerPrefs.SetInt("Player2Char" , characterIDP2);
                //change scene
                SceneManager.LoadScene("GameSceneFinal");
            }
        }
        else
        {
            timer = 4.0f;
            indicatorMessage.text = "";
        }
    }
}
