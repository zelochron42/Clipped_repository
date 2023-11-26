using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{

    [SerializeField] int zoomStepCount = 10;
    [SerializeField] float zoomStepTime = 0.05f;
    float initialFOV;
    [SerializeField] float zoomMultiplier = 2f;
    Camera cam;
    void Awake()
    {
        cam = GetComponent<Camera>();
        initialFOV = cam.fieldOfView;
    }

    void Update()
    {
        
    }

    public void DramaticZoom(bool goIn) {
        StopAllCoroutines();
        StartCoroutine("ZoomRoutine", goIn);
    }

    IEnumerator ZoomRoutine(bool goIn) {
        yield return null;
        for (int i = 0; i <= zoomStepCount; i++) {
            if (goIn)
                cam.fieldOfView = Mathf.Lerp(initialFOV, initialFOV / zoomMultiplier, (float)i / zoomStepCount);
            else
                cam.fieldOfView = Mathf.Lerp(initialFOV / zoomMultiplier, initialFOV, (float)i / zoomStepCount);
            yield return new WaitForSeconds(zoomStepTime);
        }
        yield break;
    }
}
