using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int SceneNumber;

    [SerializeField] GameObject wingModel;
    // Start is called before the first frame update
    void Start() {
        if (wingModel != null && CollectableTracker.singleton.wingsRecovered)
            wingModel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)){
            SceneManager.LoadScene(SceneNumber);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("NextScene"))
        {

            Debug.Log("Slow");
            SceneManager.LoadScene(SceneNumber);
        }
    }
}
