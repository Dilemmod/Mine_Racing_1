using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volume;
    [SerializeField] private AudioMixer masterMixer;
    private float curretVolume;

    [Header("Quality")]
    [SerializeField] private GameObject ultra=null;
    [SerializeField] private GameObject medium = null;
    [SerializeField] private GameObject low = null;

    [Space]
    [SerializeField] Dropdown qualityDropdown;
    private string[] qualityLevels;
    //[Space]
    //[SerializeField] private Toggle fullScreen;
    //[Space]
    //[SerializeField] Dropdown resolutionDropdown;
    //private Resolution[] availableResolutins;

    void Start()
    {
        volume.onValueChanged.AddListener(OnVolumeChanged);
        //fullScreen.onValueChanged.AddListener(OnFullScreenChanged);
        //resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

        masterMixer.GetFloat("Volume", out curretVolume);
        volume.value = curretVolume;

        QualityLevelController();
        /*
        availableResolutins = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentIndex = 0;
        for (int i = 0; i < availableResolutins.Length; i++) 
        {
            if (availableResolutins[i].width <= 800)
                continue;

            options.Add(availableResolutins[i].width + "x" + availableResolutins[i].height);
            if (availableResolutins[i].width == Screen.currentResolution.width
                && availableResolutins[i].height == Screen.currentResolution.height)
                currentIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
        */
        qualityLevels = QualitySettings.names;
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualityLevels.ToList());
        int qulityLvl = QualitySettings.GetQualityLevel();
        qualityDropdown.value = qulityLvl;
        qualityDropdown.RefreshShownValue();

    }
    private void OnDestroy()
    {
        volume.onValueChanged.RemoveListener(OnVolumeChanged);
        //fullScreen.onValueChanged.RemoveListener(OnFullScreenChanged);
        //resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);

    }
    /*
    private void OnResolutionChanged(int resolutionIndex)
    {
        //Resolution resolution = availableResolutins[resolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    private void OnFullScreenChanged(bool value)
    {
        Screen.fullScreen = value;
    }*/
    private void OnVolumeChanged(float volume)
    {
        masterMixer.SetFloat("Volume", volume);
    }
    private void OnQualityChanged(int qualityLvl)
    {
        QualitySettings.SetQualityLevel(qualityLvl,true);
        QualityLevelController();
    }
    public void QualityLevelController()
    {
        if (low == null || medium == null || ultra == null)
            return;
        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                low.gameObject.SetActive(false);
                medium.gameObject.SetActive(false);
                ultra.gameObject.SetActive(false);
                break;
            case 1:
                low.gameObject.SetActive(true);
                medium.gameObject.SetActive(false);
                ultra.gameObject.SetActive(false);
                break;
            case 2:
            case 3:
                low.gameObject.SetActive(true);
                medium.gameObject.SetActive(true);
                ultra.gameObject.SetActive(false);
                break;
            case 4:
            case 5:
                low.gameObject.SetActive(true);
                medium.gameObject.SetActive(true);
                ultra.gameObject.SetActive(true);
                break;
        }
    }
}
