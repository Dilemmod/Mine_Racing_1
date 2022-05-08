using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuController : BaseGameMenuController
{
    [Header("Buttons")]
    [SerializeField] private Button openMenu;
    [SerializeField] private Button restartGameMenu;
    [SerializeField] private Button restartGameOver;
    [SerializeField] private Button backToMenu;
    private bool timeGoForward;

    [Header("GameOver")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] public Text gameOverHeader;
    [SerializeField] public Text DistanceValue;
    [SerializeField] public Text CoinsValue;
    [SerializeField] public Text RecordValue;

    #region Singleton
    public static InGameMenuController Instance;

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
        //Auio
        audioManager.Play(UIClipName.BackgroundMusicGameMenu);

        openMenu.onClick.AddListener(OnOpenMenuClicked);
        play.onClick.AddListener(OnChangeMenuStatusClicked);
        restartGameMenu.onClick.AddListener(OnRestartClicked);
        restartGameOver.onClick.AddListener(OnRestartClicked);
        backToMenu.onClick.AddListener(OnGoToMainMenuClicked);
        //Run time
        TimeScale();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        play.onClick.RemoveListener(OnChangeMenuStatusClicked);
        restartGameMenu.onClick.RemoveListener(OnRestartClicked);
        restartGameOver.onClick.RemoveListener(OnRestartClicked);
        backToMenu.onClick.RemoveListener(OnGoToMainMenuClicked);
    }
    private void TimeScale()
    {
        if ((menu.activeInHierarchy || gameOverMenu.activeInHierarchy)&& !timeGoForward) 
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private void OnOpenMenuClicked()
    {
        OnChangeMenuStatusClicked();
    }
    private void OnRestartClicked()
    {
        timeGoForward = true;
        TimeScale();
        SceneTransition.Restart();
    }

    public void OnGoToMainMenuClicked()
    {
        timeGoForward = true;
        OnChangeMenuStatusClicked();
        SceneTransition.SwitchToScene(0);
    }
    protected override void OnChangeMenuStatusClicked()
    {
        base.OnChangeMenuStatusClicked();
        TimeScale();
    }
    public void OnPlayerDeath()
    {
        audioManager.Stop(UIClipName.BackgroundMusicGameMenu);
        audioManager.Stop(UIClipName.Engine);
        //-3.4 2.8 -14
        //-2.4 2 -8
        //PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex+"PlayerRecord", );
        RecordValue.text = PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex + "PlayerRecord").ToString();
        int record = Convert.ToInt32(RecordValue.text);
        int distanse = Convert.ToInt32(DistanceValue.text);
        if (record <= distanse) 
        {
            gameOverHeader.text = "NEW RECORD!";
            gameOverHeader.color = new Color(135,148, 37);
            RecordValue.text = distanse.ToString();
            audioManager.Play(UIClipName.Сongratulations);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex + "PlayerRecord", distanse);
        }
        RecordValue.text += "m";
        DistanceValue.text += "m";
        gameOverMenu.SetActive(true);
        TimeScale();
    }
}
