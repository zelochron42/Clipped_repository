using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSpawner : MonoBehaviour
{
    [SerializeField] Rigidbody2D prefab;
    [SerializeField] float velocity = 1f;
    [SerializeField] float lifetime = 1f;
    [SerializeField] float spawnInterval = 1f;
    float timeSinceSpawn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceSpawn += Time.fixedDeltaTime;
        if (timeSinceSpawn >= spawnInterval) {
            timeSinceSpawn -= spawnInterval;
            SpawnPrefab();
        }
    }

    void SpawnPrefab() {
        if (prefab == null) {
            Debug.LogError("This object spawner has no prefab assigned to it!");
            return;
        }
        Rigidbody2D newPrefab = Instantiate(prefab);
        newPrefab.transform.position = transform.position;
        newPrefab.velocity = transform.up * velocity;
        Destroy(newPrefab.gameObject, lifetime);
    }
}
