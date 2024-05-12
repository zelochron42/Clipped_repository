using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSectionSpawner : MonoBehaviour {
    private float SpawnPoint = -47;
    public GameObject[] RandomSection;
    private int RandomNo;
    [SerializeField] private int NoOfSections;
    private SectionSpeed SpeedObject;
    [SerializeField] private GameObject SpawnedFeathers;
    private int RandomFeatherNo;


    public Transform LatestSection;

    // Start is called before the first frame update
    void Start() {
        SpeedObject = FindObjectOfType<SectionSpeed>();
    }

    // Update is called once per frame
    void Update() {
        RandomNo = Random.Range(0, NoOfSections);
        if (LatestSection.position.z <= SpawnPoint) {
            NoOfSections = RandomSection.Length;
            
            GameObject SpawnedSection;
            if (SpeedObject.SectionSpeedIncrease == false) {
                SpawnedSection = Instantiate(RandomSection[RandomNo], new Vector3(0, -60, 139), Quaternion.identity) as GameObject;
                moveSection newSection = SpawnedSection.GetComponent<moveSection>();
                newSection.RiseOnStart = true;
                RandomFeatherNo = Random.Range(0, newSection.Spawnpoints.Length);
                Vector3 FeatherSpawnPoint = newSection.Spawnpoints[RandomFeatherNo].transform.position;
                GameObject SpawnedFeather = Instantiate(SpawnedFeathers, FeatherSpawnPoint, Quaternion.identity);
                SpawnedFeather.transform.parent = SpawnedSection.transform;
                LatestSection = SpawnedSection.transform;
            }
            else {
                SpawnedSection = Instantiate(RandomSection[2], new Vector3(0, -60, 139), Quaternion.identity) as GameObject;
                SpawnedSection.GetComponent<moveSection>().RiseOnStart = true;
                LatestSection = SpawnedSection.transform;
            }
        }
    }
}
