using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSkipper : MonoBehaviour
{
    public static LevelSkipper singleton;
    // Start is called before the first frame update
    void Start()
    {
        if (singleton != null && singleton != this) {
            Destroy(gameObject);
            return;
        }
        else {
            singleton = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) {
            LoadNextLevel();
            return;
        }
        else if (Input.GetKeyDown(KeyCode.F2)) {
            LoadMenu();
            return;
        }
    }
    public void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }
}
