using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SectionSpeed : MonoBehaviour
{
   public float speed;
    private FalcionPlayerScript StaminaCount;
    public bool SectionSpeedIncrease;
    public UnityEvent speedDecrease;
    //[SerializeField] [Tooltip("Limited Boost time")] private bool BoostTime = 10f;
    void Start()
    {
       StaminaCount = FindObjectOfType<FalcionPlayerScript>();
    }

    void Update()
    {
       // StartCoroutine(LimitedSpeedBoost());

        if (Input.GetButton("Fire2") && StaminaCount.StaminaAmount >= 250)
        {
            speed = 100;
            /* if (!SpeedParticle.isPlaying)
             {
                 SpeedParticle.Play();
             }*/
            SectionSpeedIncrease = true;
        }
       /* else if (Input.GetKey(KeyCode.X) && SectionSpeedIncrease)
        {
            speed = 50;
            /* if (!SpeedParticle.isPlaying)
             {
                 SpeedParticle.Play();
             }
           
        }*/
        else if (StaminaCount.StaminaAmount <= 249)
        {
            speed = 30;
            if (SectionSpeedIncrease)
                speedDecrease.Invoke();
            SectionSpeedIncrease = false;
        }
        else 
        { 
            speed = 50;
            if (SectionSpeedIncrease)
                speedDecrease.Invoke();
            SectionSpeedIncrease = false;
        }
       

    }
   
}
