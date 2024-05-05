using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpeed : MonoBehaviour
{
    public float speed = 30;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            speed = 100;
           /* if (!SpeedParticle.isPlaying)
            {
                SpeedParticle.Play();
            }*/
        }
        
    }
}
