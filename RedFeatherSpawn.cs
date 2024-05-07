using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFeatherSpawn : MonoBehaviour
{
    public GameObject[] Spawnpoints;
    [SerializeField] private GameObject SpawnedFeathers;
    [SerializeField] private Transform ParentObject;
    private int RandomNo;


    // Start is called before the first frame update
    void Start()
    { 
       
      Vector3 SpawnPoint = Spawnpoints[RandomNo].transform.position;
      SpawnedFeathers = Instantiate(SpawnedFeathers, 
          SpawnPoint, 
          Quaternion.identity, 
          ParentObject);

    }
    void Update()
    {
        RandomNo = Random.Range(0, 6);
        
        
    }
}
    
 








