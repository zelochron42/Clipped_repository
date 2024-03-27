using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSLowScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float slowMotionScale;
    public float ReverseMotionScale;
    private float startTimeScale;
    private float startFixedDeltaTime;
    public float transitionTime;
    public float BeginSlowCamTime;
    public float StopSlowCamTime;
    public AudioSource AudioSource;
    public bool SlowMotion;
    void Start()
    {
        SlowMotion = false;
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        StartCoroutine(SlowMotionTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (SlowMotion)
        {
             AudioSource.pitch = 0.1f;
        }
        else if(!SlowMotion)
        {
            AudioSource.pitch = 0.7f;

        }
    }
    IEnumerator SlowMotionTimer()

    {
        yield return new WaitForSeconds(BeginSlowCamTime);
        StartSlow();
       
        yield return new WaitForSeconds(StopSlowCamTime);
        StopSlow();
        


    }
    void StartSlow()
    {
        SlowMotion = true;
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionScale;
       
    }
    void StopSlow()
    {
        SlowMotion = false;
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
        
    }
    void ReverseTime()
    {
        Time.timeScale = ReverseMotionScale;
        Time.fixedDeltaTime = startFixedDeltaTime * ReverseMotionScale;
    }
}
