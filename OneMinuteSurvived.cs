using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneMinuteSurvived : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeRemaining = 10f;
    public int sceneNumber;

    void Update()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if(timeRemaining == 0f)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}

