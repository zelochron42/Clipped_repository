using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introcolliders : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
  

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SlowMotionTrigger"))
        {
            Cameratwo();
        }
        if (collision.gameObject.CompareTag("Camera3Trigger"))
        {
            Camerathree();
        }
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
}
