using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSection : MonoBehaviour
{
    public float speed = 30;
    public bool RiseOnStart = true;

    
    private float DestroyPoint = -200;
    private float SpawnPoint = -47;
    private bool SectionSpawned = false;
    private GameObject SpawnedSection;
    public GameObject[] RandomSection;
    private int RandomNo;
    [SerializeField] private int NoOfSections;
    private SectionSpeed SpeedObject;
    public GameObject[] Spawnpoints;
    [SerializeField] private GameObject SpawnedFeathers;
    [SerializeField] private Transform ParentObject;
    private int RandomFeatherNo;
    private Vector3 FeatherSpawnPoint;






    // Start is called before the first frame update
    void Start()
    {
       SpeedObject = FindObjectOfType<SectionSpeed>();
        Destroy(gameObject, 35);
        if(RiseOnStart)
            StartCoroutine(Rise());
        /*
        NoOfSections = RandomSection.Length;
        RandomFeatherNo = Random.Range(0, 5);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
        
        RandomNo = Random.Range(0, NoOfSections);
        speed = SpeedObject.speed;
        transform.position += new Vector3(0, 0, -2) * speed * Time.deltaTime;
        //speed += (0.01f * 0.3f);
     
            
            /*if (!SpeedParticle.isPlaying)
            {
                SpeedParticle.Play();
            }*/
 
        /*if(transform.position.z <= SpawnPoint && !SectionSpawned)
        {
            if(SpeedObject.SectionSpeedIncrease)
            {
                SpawnedSection = Instantiate(RandomSection[RandomNo], new Vector3(0, -60, 139), Quaternion.identity) as GameObject;
                moveSection newSection = SpawnedSection.GetComponent<moveSection>();
                newSection.RiseOnStart = true;
                FeatherSpawnPoint = newSection.Spawnpoints[RandomFeatherNo].transform.position;
                GameObject SpawnedFeather = Instantiate(SpawnedFeathers, FeatherSpawnPoint, Quaternion.identity);
                SpawnedFeather.transform.parent = SpawnedSection.transform;
            }
            else if(!SpeedObject.SectionSpeedIncrease)
            {
                SpawnedSection = Instantiate(RandomSection[2], new Vector3(0, -60, 139), Quaternion.identity) as GameObject;
                SpawnedSection.GetComponent<moveSection>().RiseOnStart = true;
            }
            
            SectionSpawned = true;
          

       
        }*/
        if(transform.position.z <= DestroyPoint)
        {
            Destroy(gameObject);
        }

    }
    IEnumerator Rise()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.Translate(0f, 3f, 0f);
            if (SpeedObject.speed > 50) 
                yield return new WaitForSeconds(0.001f);
            else
                yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
  
 
}
