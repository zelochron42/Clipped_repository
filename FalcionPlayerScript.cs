using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FalcionPlayerScript : MonoBehaviour
{
    [Header("Player Stats")]
    public float PLayerHealth = 30;
    public float StaminaAmount = 100;


    [SerializeField] [Tooltip("Amount of Stamina Lost per frame")] public float StaminaLoss;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider StaminaBar;
    [SerializeField] private Image StaminaFill;
    private SectionSpeed SpeedObject;
    private string currentSceneName;




    //variables for invincibility blink effect
    List<Renderer> renders = new List<Renderer>();
    [SerializeField] [Tooltip("Duration of each blink while invulnerable")] float blinkTime = 0.2f;
    [SerializeField] [Tooltip("Duration of invulnerability after taking damage")] float invulnTime = 1f;
    [SerializeField] bool invulnerable = false;
    [SerializeField] bool DashInvul = false;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = PLayerHealth;
        SpeedObject = FindObjectOfType<SectionSpeed>();

        //invulnerability code
        Renderer[] allRenders = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allRenders) {
            if (r.enabled) {
                renders.Add(r); //add active renderers to the list of renderers that will be toggled while invulnerable
            }
        }
        currentSceneName = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {
        

        //StaminaCheck();
        SliderColorChange();
        if(PLayerHealth <= 0)
        {

            SceneManager.LoadScene(currentSceneName);

        } 
        healthBar.value = PLayerHealth;
        StaminaBar.value = StaminaAmount;
        StaminaAmount -= StaminaLoss;
        BlinkEffect();
        if (Input.GetKey(KeyCode.Q) && StaminaAmount >=750)
        {
            DashInvul = true;
            blinkTime = 0.05f;
            StaminaLoss = 1f;
        }
        if (Input.GetKey(KeyCode.E) || SpeedObject.speed <= 50)
        {
            Debug.Log("E Pressed!");
            DashInvul = false;
            blinkTime = 0.1f;
            StaminaLoss = 0.5f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage") && !invulnerable && !DashInvul)
        {
            invulnerable = true;
            StartCoroutine("InvulnBlink");
            PLayerHealth -= 1;
            SpeedObject.speed = -5;
        }
        if (other.gameObject.CompareTag("Stamina"))
        {
          
             StaminaAmount += 100;
            if (StaminaAmount >= 1000)
            {
                StaminaAmount = 1000;
            }
            if(SpeedObject.speed >= 51)
            {
                SpeedObject.speed += 0.05f;
            }
            
        }
        if (other.gameObject.CompareTag("NextSceneTrigger"))
        {

            SceneManager.LoadScene(7);

        }


    }

    
    void BlinkEffect() {
        if (invulnerable || DashInvul)
            SetRenderers((int)Mathf.Floor(Time.time / blinkTime) % 2 == 0);
        else
            SetRenderers(true);
    }

    void SetRenderers(bool enabled) {
        foreach (Renderer r in renders)
            r.enabled = enabled;
    }

    IEnumerator InvulnBlink() {
        yield return new WaitForSeconds(invulnTime);
        invulnerable = false;
        yield break;
    }
    void SliderColorChange()
    {
        if (StaminaAmount >= 750)
        {
            StaminaFill.color = Color.green;
        }
        else if (StaminaAmount >= 250 && StaminaAmount <= 749)
        {
            StaminaFill.color = Color.yellow;
        }
        else if (StaminaAmount <= 249)
        {
            StaminaFill.color = Color.red;
        }
    }
    void StaminaCheck()
    {
        if(StaminaAmount <= 0)
        {
            PLayerHealth -= 0.1f;
            
        }
    }
    

}
