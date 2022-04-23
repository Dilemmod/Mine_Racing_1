using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static CameraControllerMainMenu;
using System;

public class MainMenuController : BaseGameMenuController
{
    [Header("Player menu")]
    [SerializeField] private GameObject playerMenuButtons;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject tuningMenu;
    [SerializeField] private GameObject playerMenu;
    [SerializeField] protected Button buttonBack;
    [SerializeField] protected Button buttonToLevelMenu;
    [SerializeField] protected Button buttonToTuningMenu;

    [Header("Animators")]
    [SerializeField] private Animator levelMenuAnimator;
    [SerializeField] private Animator tuningMenuAnimator;
    [SerializeField] private Animator playerMenuAnimator;
    [SerializeField] private Animator mainMenuAnimator;

    private CameraControllerMainMenu cameraControllerMainMenu;
    
    protected override void Start()
    {
        base.Start();
        cameraControllerMainMenu = Instance;
        play.onClick.AddListener(OnPlayClecked);
        buttonBack.onClick.AddListener(OnBackClecked);
        buttonToLevelMenu.onClick.AddListener(OnLevelMenuClecked);
        buttonToTuningMenu.onClick.AddListener(OnTuningMenuClecked);
        if (PlayerPrefs.HasKey("PlayerCoins"))
            playerMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>().text = PlayerPrefs.GetInt("PlayerCoins").ToString();
        audioManager.Play(UIClipName.BackgroundMusic);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        buttonBack.onClick.RemoveListener(OnBackClecked);
        play.onClick.RemoveListener(OnPlayClecked);
        buttonToLevelMenu.onClick.RemoveListener(OnLevelMenuClecked);
        buttonToTuningMenu.onClick.RemoveListener(OnTuningMenuClecked);
    }
    private void OnPlayClecked()
    {
        mainMenuAnimator.SetTrigger("Close");
        playerMenuAnimator.SetTrigger("Open");
        cameraControllerMainMenu.CameraPosition(MenuPosition.playerTarget);
        OnChangePlayerMenuStatus();
        audioManager.Play(UIClipName.Play);
    }
    private void OnBackClecked()
    {
        switch (cameraControllerMainMenu.moveTo)
        {
            case (MenuPosition.playerTarget):
                mainMenuAnimator.SetTrigger("Open");
                playerMenuAnimator.SetTrigger("Close");
                cameraControllerMainMenu.CameraPosition(MenuPosition.mainTarget);
                break;
            case (MenuPosition.levelTarget):
                levelMenuAnimator.SetTrigger("Close");
                cameraControllerMainMenu.CameraPosition(MenuPosition.playerTarget);
                break;
            case (MenuPosition.tuningTarget):
                tuningMenuAnimator.SetTrigger("Close");
                cameraControllerMainMenu.CameraPosition(MenuPosition.playerTarget);
                break;
        }
        audioManager.Play(UIClipName.Quit);
        OnChangePlayerMenuStatus();
    }
    private void OnLevelMenuClecked()
    {
        cameraControllerMainMenu.CameraPosition(MenuPosition.levelTarget);
        audioManager.Play(UIClipName.LvlMenu);
        levelMenuAnimator.SetTrigger("Open");
        OnChangePlayerMenuStatus();
    }
    private void OnTuningMenuClecked()
    {
        cameraControllerMainMenu.CameraPosition(MenuPosition.tuningTarget);
        audioManager.Play(UIClipName.LvlMenu);
        tuningMenuAnimator.SetTrigger("Open");
        OnChangePlayerMenuStatus();
    }
    private void OnChangePlayerMenuStatus()
    {
        playerMenuButtons.SetActive(!playerMenuButtons.activeInHierarchy);
    }
}
