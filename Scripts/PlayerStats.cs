using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int DamageAmount;
    public float PlayerHealth = 100;
    public Slider HealthSlider;
    // Start is called before the first frame update
    void Start()
    {
        HealthSlider.value = PlayerHealth;

    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = PlayerHealth;
        if(PlayerHealth <= 0)
        {
            Debug.Log("Player is big dead");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            PlayerHealth -=DamageAmount;
            Debug.Log("Ouch");
        }
    }
    public void Damage()
    {
        PlayerHealth -= DamageAmount;
        Debug.Log("Ouchie");
    }
}
