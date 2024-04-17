using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    void Start()
    {
        CameraOne();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Camera1"))
        {
            CameraOne();
        }
        else if (other.gameObject.CompareTag("Camera2"))
        {
            Cameratwo();
        }
        else if (other.gameObject.CompareTag("Camera3"))
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
