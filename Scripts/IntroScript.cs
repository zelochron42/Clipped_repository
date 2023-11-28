using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float slowMotionScale;
    public float ReverseMotionScale;
    private float startTimeScale;
    private float startFixedDeltaTime;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public Animator transition;
    public float transitionTime;



    void Start()
    {
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
  
    }

    // Update is called once per frame
    void Update()
     {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSlow();
            Debug.Log("Hello");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopSlow();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ReverseTime();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ReverseTime();
        }
     }
        private void OnTriggerEnter(Collider collision)
        {
        if (collision.gameObject.CompareTag("SlowMotionTrigger"))
        {

            
            StartSlow();
        }
        if (collision.gameObject.CompareTag("NextScene"))
        {


            LoadNextLevel();
        }
        if (collision.gameObject.CompareTag("SlowDownTrigger"))
            {
               
            Cameratwo();
             StartSlow();
            }
        if (collision.gameObject.CompareTag("Camera3Trigger"))
        {
           
            Camerathree();

        }



    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("SlowDownTrigger"))
        {
            StopSlow();
        }
        if (collision.gameObject.CompareTag("SlowMotionTrigger"))
        {
            StopSlow();
        }

    }
        void StartSlow()
        {
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = startFixedDeltaTime * slowMotionScale;
        }
        void StopSlow()
        {
            Time.timeScale = startTimeScale;
            Time.fixedDeltaTime = startFixedDeltaTime;
        }
        void ReverseTime()
        {
            Time.timeScale = ReverseMotionScale;
            Time.fixedDeltaTime = startFixedDeltaTime * ReverseMotionScale;
        }

    private void CameraOne()
    {
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        Camera3.SetActive(false);
    }
    private void Cameratwo()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        Camera3.SetActive(false);
    }
    private void Camerathree()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(true);
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }



}