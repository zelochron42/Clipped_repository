using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalcionMoveScript : MonoBehaviour
{
    public float speed = 30;
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
            speed = -1.5f;
        }
        else if(SpeedObject.speed >= 100)
        {
            speed = -3f;
        }
        else
            speed = 1.5f;
    }
}
