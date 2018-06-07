using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public float splashTime = 5f;

    void Update()
    {
        if(Time.time >= splashTime)
        {
            SceneManager.LoadScene("PlayerSelectionScene");
        }
    }
}
