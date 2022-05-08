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
    [SerializeField] protected Button quit;

    [Header("Settings")]
    [SerializeField] protected GameObject settingsMenu;
    [SerializeField] protected Button closeSettings;

   // [Obsolete]
    protected virtual void Start()
    {
        audioManager = UIAudioManager.Instance;
        settingsManager = SettingsMenuController.Instance;
        quit.onClick.AddListener(OnQuitClicked);
        settings.onClick.AddListener(OnSettingsClicked);
        closeSettings.onClick.AddListener(OnSettingsClicked);

        //PlayerPrefsValue
        if (PlayerPrefs.HasKey("PlayerVolume"))
            settingsManager.SetVolume(PlayerPrefs.GetFloat("PlayerVolume"));
        else
            settingsManager.SetVolume(-30);
        if (PlayerPrefs.HasKey("PlayerQuality"))
            settingsManager.SetQuality(PlayerPrefs.GetInt("PlayerQuality"));
        else
            settingsManager.SetQuality(settingsManager.GetQuality());
    }
    protected virtual void OnDestroy()
    {
        quit.onClick.RemoveListener(OnQuitClicked);
        settings.onClick.RemoveListener(OnSettingsClicked);
        closeSettings.onClick.RemoveListener(OnSettingsClicked);
    }
    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            OnChangeMenuStatusClicked();
    }
  
    protected virtual void OnChangeMenuStatusClicked()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }
    protected virtual void OnSettingsClicked()
    {
        audioManager.Play(UIClipName.Settings);
        settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
    }
    private void OnQuitClicked()
    {
        audioManager.Play(UIClipName.Quit);
        OnChangeMenuStatusClicked();
        SceneTransition.QuitToDesktop();
    }
}
