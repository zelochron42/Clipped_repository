using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWaveController : MonoBehaviour
{
    [SerializeField] Transform[] waveObjects;
    [SerializeField] float waveAmplitude;
    [SerializeField] float waveFrequency;
    [SerializeField] float waveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time * waveSpeed;
        for (int i = 0; i < waveObjects.Length; i++) {
            float waveOffset = Mathf.Sin(currentTime + i * waveFrequency / 10f) * waveAmplitude;
            waveObjects[i].position = new Vector2(waveObjects[i].position.x, transform.position.y + waveOffset);
        }
    }
}
