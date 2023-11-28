using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;
    public float camShakeDuration;
    public float camShakeAmount;
    public float decrementFactor;
    private Vector3 _camOriginalPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        OnEnable();
        if(camShakeDuration > 0)
        {
            camTransform.localPosition = _camOriginalPosition + Random.insideUnitSphere * camShakeAmount;
            camShakeDuration -= Time.deltaTime * decrementFactor;
        }
        else
        {
            camShakeDuration = 0f;
            camTransform.localPosition = _camOriginalPosition;
        }
    }
    private void OnEnable()
    {
        _camOriginalPosition = camTransform.position;
    }
  
}
