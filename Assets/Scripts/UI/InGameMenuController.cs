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

    //MessageBox
    private Animator messageBox;
    private GameObject messageBoxObject;

    private CarController carController;
    private InterAd interAd;
    private int playerTryCount;
    private int maximumPlayerTry = 6;//4 try

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
        //Ad
        interAd = gameObject.GetComponent<InterAd>();
        carController = CarController.Instance;
        playerTryCount = PlayerPrefs.GetInt("playerTryCount");
        //Listeners
        openMenu.onClick.AddListener(OnOpenMenuClicked);
        play.onClick.AddListener(OnChangeMenuStatusClicked);
        restartGameMenu.onClick.AddListener(OnRestartClicked);
        restartGameOver.onClick.AddListener(OnRestartClicked);
        backToMenu.onClick.AddListener(OnGoToMainMenuClicked);
        //Auio
        audioManager.Play(UIClipName.BackgroundMusicGameMenu);
        //Run time
        TimeScale();
        //Fix bug)
        if (playerTryCount == 1)
            playerTryCount = 2;

        //MessageBox
        messageBoxObject = GameObject.FindGameObjectWithTag("MessageBox");
        messageBox = messageBoxObject.GetComponent<Animator>() ;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            OnChangeMenuStatusClicked();
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
        if ((menu.activeInHierarchy || gameOverMenu.activeInHierarchy) && !timeGoForward)
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
        OnPlayerDeath();
        timeGoForward = true;
        TimeScale();
        SceneTransition.Restart();
    }

    public void OnGoToMainMenuClicked()
    {
        OnPlayerDeath();
        timeGoForward = true;
        OnChangeMenuStatusClicked();
        SceneTransition.SwitchToScene(0);
    }
    private void OnChangeMenuStatusClicked()
    {
        menu.SetActive(!menu.activeInHierarchy);
        TimeScale();
    }
    public void OnPlayerDeath()
    {
        //Ad
        if (playerTryCount >= maximumPlayerTry)
        {
            //Temporarily
            //interAd.ShowAd();
            playerTryCount = 0;
            PlayerPrefs.SetInt("playerTryCount", playerTryCount);
            //StartCoroutine(adClosing());
        }
        else
        {
           
            playerTryCount++;
            PlayerPrefs.SetInt("playerTryCount", playerTryCount);
        }
        DeathMenu();
    }
    private void DeathMenu()
    {
        carController.OnDeath();
        //Audio
        audioManager.Stop(UIClipName.BackgroundMusicGameMenu);
        audioManager.Stop(UIClipName.Engine);
        RecordValue.text = PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex + "PlayerRecord").ToString();
        int record = Convert.ToInt32(RecordValue.text);
        int distanse = Convert.ToInt32(DistanceValue.text);
        if (record <= distanse)
        {
            gameOverHeader.text = "NEW RECORD!";
            gameOverHeader.color = new Color(225, 229, 0);
            RecordValue.text = distanse.ToString();
            audioManager.Play(UIClipName.Сongratulations);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex + "PlayerRecord", distanse);
        }
        RecordValue.text += "m";
        DistanceValue.text += "m";
        gameOverMenu.SetActive(true);
        TimeScale();
    }
    IEnumerator adClosing()
    {
        yield return new WaitUntil(() => interAd.adClose == true);
        DeathMenu();
    }
    public void ShowMassageBox(string massage)
    {
        messageBox.SetTrigger("Open");
        messageBoxObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = massage;
        int x = UnityEngine.Random.Range(-150, 300);
        int y = UnityEngine.Random.Range(-420, -300);
        messageBoxObject.transform.localPosition = new Vector3(x, y, 0);
    }
}
