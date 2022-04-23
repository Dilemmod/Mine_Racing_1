using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected UIAudioManager audioManager;
    [SerializeField] protected GameObject menu;

    [Header("MainButttons")]
    [SerializeField] protected Button play;
    [SerializeField] protected Button settings;
    [SerializeField] protected Button quit;

    [Header("Settings")]
    [SerializeField] protected GameObject settingsMenu;
    [SerializeField] protected Button closeSettings;
    
    protected virtual void Start()
    {
        audioManager = UIAudioManager.Instance;
        quit.onClick.AddListener(OnQuitClicked);
        settings.onClick.AddListener(OnSettingsClicked);
        closeSettings.onClick.AddListener(OnSettingsClicked);
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
