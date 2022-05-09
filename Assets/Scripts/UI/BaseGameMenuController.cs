using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected UIAudioManager audioManager;
    protected SettingsMenuController settingsManager;
    [SerializeField] protected GameObject menu;

    [Header("MainButttons")]
    [SerializeField] protected Button play;
    [SerializeField] protected Button settings;

    [Header("Settings")]
    [SerializeField] protected GameObject settingsMenu;
    [SerializeField] protected Button closeSettings;

   // [Obsolete]
    protected virtual void Start()
    {
        audioManager = UIAudioManager.Instance;
        settingsManager = SettingsMenuController.Instance;
        settings.onClick.AddListener(OnSettingsClicked);
        closeSettings.onClick.AddListener(OnSettingsClicked);

        //PlayerPrefsValue
        if (PlayerPrefs.HasKey("PlayerVolume"))
            settingsManager.SetVolume(PlayerPrefs.GetFloat("PlayerVolume"));
        else
            settingsManager.SetVolume(-20);
        if (PlayerPrefs.HasKey("PlayerQuality"))
            settingsManager.SetQuality(PlayerPrefs.GetInt("PlayerQuality"));
        else
            settingsManager.SetQuality(settingsManager.GetQuality());
    }
    protected virtual void OnDestroy()
    {
        settings.onClick.RemoveListener(OnSettingsClicked);
        closeSettings.onClick.RemoveListener(OnSettingsClicked);
    }
    protected virtual void OnSettingsClicked()
    {
        audioManager.Play(UIClipName.Settings);
        settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
    }
}
