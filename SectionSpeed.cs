using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpeed : MonoBehaviour
{
    public float speed = 50;
    private FalcionPlayerScript StaminaCount;
    public bool SectionSpeedIncrease;
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
            SectionSpeedIncrease = true;
        }
        else if (Input.GetKey(KeyCode.E) || StaminaCount.StaminaAmount <=249)
        {
            speed = 30;
            /* if (!SpeedParticle.isPlaying)
             {
                 SpeedParticle.Play();
             }*/
            SectionSpeedIncrease = false;
        }
        if (StaminaCount.StaminaAmount <= 250)
        {
            speed = 30;
        }
        if (StaminaCount.StaminaAmount <= 0)
        {
            speed -= 0.1f;
            Debug.Log("StaminaDecreases");
        }

    }
}
