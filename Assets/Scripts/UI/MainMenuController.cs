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
    public Text playerCoinsText { get; set; }

    #region Singleton
    public static MainMenuController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    protected override void Start()
    {
        base.Start();
        //PlayerPrefs.DeleteAll();Debug.LogWarning("DEleteAll");
        //Main menu
        mainMenuAnimator.SetTrigger("Open");
        //Auio
        audioManager.Play(UIClipName.BackgroundMusicMainMenu);

        cameraControllerMainMenu = CameraControllerMainMenu.Instance;
        play.onClick.AddListener(OnPlayClicked);
        buttonBack.onClick.AddListener(OnBackClicked);
        buttonToLevelMenu.onClick.AddListener(OnLevelMenuClicked);
        buttonToTuningMenu.onClick.AddListener(OnTuningMenuClicked);
        playerCoinsText = playerMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>();
        if (!PlayerPrefs.HasKey("PlayerCoins"))
            PlayerPrefs.SetInt("PlayerCoins", 0);
        //Debug.LogWarning("PlayerCoins = 5000");
        playerCoinsText.text = PlayerPrefs.GetInt("PlayerCoins").ToString();
        //playerMenu.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>().text =
        //PlayerPrefs.SetInt("PlayerCoins", 0);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        buttonBack.onClick.RemoveListener(OnBackClicked);
        play.onClick.RemoveListener(OnPlayClicked);
        buttonToLevelMenu.onClick.RemoveListener(OnLevelMenuClicked);
        buttonToTuningMenu.onClick.RemoveListener(OnTuningMenuClicked);
    }
    private void OnPlayClicked()
    {
        mainMenuAnimator.SetTrigger("Close");
        playerMenuAnimator.SetTrigger("Open");
        cameraControllerMainMenu.CameraPosition(MenuPosition.playerTarget);
        OnChangePlayerMenuStatus();
        audioManager.Play(UIClipName.Play);
    }
    private void OnBackClicked()
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
    private void OnLevelMenuClicked()
    {
        cameraControllerMainMenu.CameraPosition(MenuPosition.levelTarget);
        audioManager.Play(UIClipName.LvlMenu);
        levelMenuAnimator.SetTrigger("Open");
        OnChangePlayerMenuStatus();
    }
    private void OnTuningMenuClicked()
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
