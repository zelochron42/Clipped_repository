using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalcionInstructions : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject[] UIItems;
    [SerializeField] int currentElement = 0;
    [SerializeField] private GameObject Instructions;
    [SerializeField] private GameObject LevelExplanation;
    [SerializeField] private GameObject LevelMechanics;
    [SerializeField] private GameObject LevelControls;
    private bool InstructionsActive;
    private bool ExplanationActive;
    private bool MechanicsActive;
    private bool ControlsActive;
    private FalcionPlayerScript FalcionPlayer;
    void Start()
    {
        Time.timeScale = 0f;
        InstructionsActive = true;
        /*
        Instructions.SetActive(true);
        ExplanationActive = true;
        LevelExplanation.SetActive(true);
        */
        UIItems[currentElement].SetActive(true);
        FalcionPlayer = FindObjectOfType<FalcionPlayerScript>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!InstructionsActive) {
            return;
        }
        if (FalcionPlayer.StaminaAmount <= 449) {
            FalcionPlayer.StaminaAmount = 450;
        }
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump")) {
            UIItems[currentElement].SetActive(false);
            currentElement++;
            if (currentElement >= UIItems.Length) {
                Time.timeScale = 1f;
                InstructionsActive = false;
            }
            else {
                UIItems[currentElement].SetActive(true);
            }
        }
        /*
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && ExplanationActive)
        {
            ExplanationActive = false;
            LevelExplanation.SetActive(false);
            //Destroy(LevelExplanation);
            LevelMechanics.SetActive(true);
            MechanicsActive = true;
        }
        else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && MechanicsActive)
        {
            LevelMechanics.SetActive(false);
            MechanicsActive = false;
            //Destroy(LevelMechanics);
            LevelControls.SetActive(true);
            ControlsActive = true;
        }
        else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && ControlsActive)
        {
            LevelControls.SetActive(false);
            ControlsActive = false;
            //Destroy(LevelControls);
            Time.timeScale = 1f;
            InstructionsActive = false;
        }
        */
    }
 
}
