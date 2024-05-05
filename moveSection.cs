using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSection : MonoBehaviour
{
    public float speed = 30;
    public bool RiseOnStart = true;
    [SerializeField] GameObject Falcion;
    [SerializeField] ParticleSystem SpeedParticle;
    private float DestroyPoint = -313;
    private float SpawnPoint = -47;
    private bool SectionSpawned = false;
    private GameObject SpawnedSection;
    public GameObject[] RandomSection;
    private int RandomNo;
    [SerializeField] private int NoOfSections;
    private SectionSpeed SpeedObject;
 
    



    // Start is called before the first frame update
    void Start()
    {
       SpeedObject = FindObjectOfType<SectionSpeed>();
        Destroy(gameObject, 5);
        if(RiseOnStart)
        StartCoroutine(Rise());
        NoOfSections = RandomSection.Length;
        
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
 
        if(transform.position.z <= SpawnPoint && !SectionSpawned)
        {
            
            SpawnedSection = Instantiate(RandomSection[RandomNo], new Vector3(0, -60, 139), Quaternion.identity) as GameObject;
            SectionSpawned = true;

       
        }
        if(transform.position.z <= DestroyPoint)
        {
            Destroy(gameObject);
        }


    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Rise()
    {
        for (int i = 0; i < 20; i++)
        {
          
            transform.Translate(0f, 3f, 0f);
            if (SpeedObject.speed > 30) 
                yield return new WaitForSeconds(0.01f);
            else
                yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }

 
}
