using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Infographic : MonoBehaviour
{
    [SerializeField] GameObject[] keyboard_ui;
    [SerializeField] GameObject[] controller_ui;
    // Start is called before the first frame update
    void Start()
    {
        SetEnabledObjects(keyboard_ui, false);
        SetEnabledObjects(controller_ui, true);
    }

    void SetEnabledObjects(GameObject[] items, bool objectIsControllerUI) {
        bool setEnable = InputSettings.controller_ui ^ !objectIsControllerUI; //set UI for controllers and keyboards to visible/invisible based on input ui settings
        foreach (GameObject g in items) {
            g.SetActive(setEnable);
        }
    }
}
