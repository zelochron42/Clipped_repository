using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    List<Resolution> culledResolutions
    public TMPro.TMP_Dropdown resolutionDropDown;
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        culledResolutions = new List<Resolution>();
        int currentResolutionindex = 0;
        for(int i = 0; i< resolutions.Length; i++)
        {
            
            if (!culledResolutions.Contains(resolutions[i]) {
                culledResolutions.Add(resolutions[i]);
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);
            }
        }

        for (int i = 0; i < culledResolutions.Count; i++) {
            if (culledResolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionindex = i;
            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionindex;
        resolutionDropDown.RefreshShownValue();


    }
    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = culledResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
    public void SetQuality(int qualityindex)
    {
        QualitySettings.SetQualityLevel(qualityindex);
    }
    public void SetFullscreen(bool isfull)
    {
        Screen.fullScreen = isfull;
    }
}
