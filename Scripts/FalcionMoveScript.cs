using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalcionMoveScript : MonoBehaviour
{
    private float speed;
    private SectionSpeed SpeedObject;
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
            speed = -1.0f;
        }
        else if(SpeedObject.speed >= 100)
        {
            speed = -3f;
        }
        else
            speed = 1.0f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NextSceneTrigger"))
        {
            Time.timeScale = 0;
            Debug.Log("FalcionWins!!!1");

        }
    }
}
