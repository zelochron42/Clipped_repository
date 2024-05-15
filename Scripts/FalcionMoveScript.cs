using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FalcionMoveScript : MonoBehaviour
{
    private float speed;
    private SectionSpeed SpeedObject;
    public GameObject FalcionWinsScreen;
    private bool FailScreenAction;
    // Start is called before the first frame update
    void Start()
    {
        SpeedObject = FindObjectOfType<SectionSpeed>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, 2) * speed * Time.deltaTime;
        if (SpeedObject.speed >= 51 && SpeedObject.speed <= 70)
        {
            speed = -0.7f;
        }
        else if(SpeedObject.speed >= 100)
        {
            speed = -3f;
        }
        else
            speed = 0.7f;

        ReStartScene();
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NextSceneTrigger"))
        {
            Time.timeScale = 0;
            Debug.Log("FalcionWins!!!1");
            FalcionWinsScreen.SetActive(true);
            FailScreenAction = true;

        }
    }
    void ReStartScene()
    {
        if (FailScreenAction)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
