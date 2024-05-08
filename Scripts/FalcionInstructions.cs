using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalcionInstructions : MonoBehaviour
{
    // Start is called before the first frame update
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
        Instructions.SetActive(true);
        ExplanationActive = true;
        LevelExplanation.SetActive(true);
        FalcionPlayer = FindObjectOfType<FalcionPlayerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (InstructionsActive)
        {
            if(FalcionPlayer.StaminaAmount <= 449)
            {
                FalcionPlayer.StaminaAmount = 450;
            }
        }
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && ExplanationActive)
        {
            ExplanationActive = false;
            LevelExplanation.SetActive(false);
            LevelMechanics.SetActive(true);
            MechanicsActive = true;
        }
        else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && MechanicsActive)
        {
            LevelMechanics.SetActive(false);
            MechanicsActive = false;
            LevelControls.SetActive(true);
            ControlsActive = true;
        }
        else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && ControlsActive)
        {
            LevelControls.SetActive(false);
            ControlsActive = false;
            Time.timeScale = 1f;
            InstructionsActive = false;
        }

    }
 
}
