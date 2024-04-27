using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    // public bool ActiveTrigger;
    private int RandomNo;
    [SerializeField] private int NoOfSections;
    public GameObject SectionTriggerObj;
    private GameObject SpawnedSection;
    public GameObject[] RandomSection;
    [SerializeField] private int SectionLimit = 1;
    void Start()
    {
        NoOfSections = RandomSection.Length;
    }
    void FixedUpdate()
    {
        RandomNo = Random.Range(0, NoOfSections);
    }
   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NewSectionTrigger"))
        {
            for (int i = 0; i < SectionLimit; i++)
            {
                SpawnedSection =  Instantiate(RandomSection[RandomNo], new Vector3(0, -60, 139), Quaternion.identity) as GameObject;
                //Destroy(SectionTriggerObj);
                Destroy(other.gameObject);
                //SectionTriggerObj = SpawnedSection.transform.Find("New-Section-Trigger").gameObject;
            }
        }
    }
}
