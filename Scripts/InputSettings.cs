using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : MonoBehaviour
{
    [SerializeField] public static bool controller_ui = false;
    [SerializeField] Toggle settingButton;

    private void Start() {
        settingButton.isOn = controller_ui;
    }

    public void SetUIState(bool state) {
        controller_ui = state;
    }
}
