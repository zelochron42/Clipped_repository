using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpeed : MonoBehaviour
{
    public float speed = 50;
    private FalcionPlayerScript StaminaCount;
    public bool SectionSpeedIncrease;
    //[SerializeField] [Tooltip("Limited Boost time")] private bool BoostTime = 10f;
    void Start()
    {
       StaminaCount = FindObjectOfType<FalcionPlayerScript>();
    }

    void Update()
    {
        StartCoroutine(LimitedSpeedBoost());

            if (Input.GetKey(KeyCode.Q) && StaminaCount.StaminaAmount >= 250)
        {
            speed = 100;
            /* if (!SpeedParticle.isPlaying)
             {
                 SpeedParticle.Play();
             }*/
            SectionSpeedIncrease = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            speed = 50;
            /* if (!SpeedParticle.isPlaying)
             {
                 SpeedParticle.Play();
             }*/
            SectionSpeedIncrease = false;
        }
        if (StaminaCount.StaminaAmount <= 249)
        {
            speed = 30;
        }
        else 
        { 
            speed = 50;
        }
       

    }
    IEnumerator LimitedSpeedBoost()
    {
        if (Input.GetKey(KeyCode.Q) && StaminaCount.StaminaAmount <= 750)
        {
            speed = 70;
             yield return new WaitForSeconds(10f);
            speed = 50;
        }
    }
}
