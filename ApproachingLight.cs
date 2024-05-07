using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachingLight : MonoBehaviour
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
        transform.position += new Vector3(0, 0, -2) * speed * Time.deltaTime;
        if (SpeedObject.speed >= 51)
        {
            speed = 15f;
        }
        else
            speed = 10f;
    }
}
