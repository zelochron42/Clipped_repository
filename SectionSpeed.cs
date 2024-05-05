using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpeed : MonoBehaviour
{
    public float speed = 30;
    private FalcionPlayerScript StaminaCount;
    void Start()
    {
       StaminaCount = FindObjectOfType<FalcionPlayerScript>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && StaminaCount.StaminaAmount >= 750)
        {
            speed = 100;
           /* if (!SpeedParticle.isPlaying)
            {
                SpeedParticle.Play();
            }*/
        }
        else if (Input.GetKey(KeyCode.E))
        {
            speed = 30;
            /* if (!SpeedParticle.isPlaying)
             {
                 SpeedParticle.Play();
             }*/
        }
        if (StaminaCount.StaminaAmount <= 250)
        {
            speed = 30;
        }

    }
}
