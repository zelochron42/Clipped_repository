using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] bool drawGizmos = false;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    Transform player;
    [SerializeField] int zoomStepCount = 10;
    [SerializeField] float zoomStepTime = 0.05f;
    float initialFOV;
    [SerializeField] float zoomMultiplier = 2f;
    Camera cam;
    void Awake()
    {
        cam = GetComponent<Camera>();
        initialFOV = cam.fieldOfView;
        FindPlayer();
    }

    void Update()
    {
        
    }
    private void LateUpdate() {
        FollowPlayer();
    }

    void FollowPlayer() {
        if (!player) {
            FindPlayer();
            return;
        }
        float goalX = Mathf.Clamp(player.position.x, minX, maxX);
        float goalY = Mathf.Clamp(player.position.y, minY, maxY);
        transform.position = new Vector3(goalX, goalY, transform.position.z);
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

    void FindPlayer() {
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm)
            player = pm.transform;
    }

    private void OnDrawGizmos() {
        if (drawGizmos) {
            Gizmos.color = Color.red;
            Vector2 cubeCenter = new Vector2(minX + maxX, minY + maxY) / 2f;
            Vector2 cubeSize = new Vector2(maxX - minX, maxY - minY);
            Gizmos.DrawWireCube(cubeCenter, cubeSize);
        }
    }
}
