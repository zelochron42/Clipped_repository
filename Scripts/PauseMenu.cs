using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isPaused = false;
    public GameObject pauseMenu;
    float hitStopTime = 0.05f;

    private void Awake() {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }
    void Start()
    {
        //pauseMenu.SetActive(false)  ;
        PlayerStats ps = FindObjectOfType<PlayerStats>();
        if (ps)
            ps.DamageReceived.AddListener(HitStop);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        StopAllCoroutines();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void HitStop() {
        StopAllCoroutines();
        Time.timeScale = 0f;
        StartCoroutine("EndHitStop");
    }
    IEnumerator EndHitStop() {
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1f;
        yield break;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
