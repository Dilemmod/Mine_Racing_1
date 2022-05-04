using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Text loadingPercentage;
    [SerializeField] private Image loadingProgressBar;
    private GameObject backgroundImage;
    private GameObject loadingBlock;
    private static bool shouldPlayOpeningAnimation = false;
    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperetion;
    #region Singleton
    public static SceneTransition Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    private void Start()
    {
        backgroundImage = transform.GetChild(0).gameObject;
        loadingBlock = transform.GetChild(1).gameObject;
        componentAnimator = GetComponent<Animator>();
        if (shouldPlayOpeningAnimation) componentAnimator.SetTrigger("sceneOpening");
    }
    private void Update()
    {
        if (loadingSceneOperetion != null)
        {
            loadingPercentage.text = Mathf.RoundToInt(loadingSceneOperetion.progress * 100).ToString() + "%";
            loadingProgressBar.fillAmount = loadingSceneOperetion.progress;
        }
    }
    public static void SwitchToScene(int sceneID)
    {
        //Debug.Log("Swich");
        Instance.componentAnimator.SetTrigger("sceneClosing");
        Instance.loadingSceneOperetion = SceneManager.LoadSceneAsync(sceneID);
        Instance.loadingSceneOperetion.allowSceneActivation = false;
    }
    public static void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
       //SwitchToScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void QuitToDesktop()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
    private void OnAnimationClosingOver()
    {
        shouldPlayOpeningAnimation = true;
        loadingSceneOperetion.allowSceneActivation = true;
    }
    private void OnAnimationOpeningOver()
    {
        backgroundImage.SetActive(false);
        loadingBlock.SetActive(false);
    }
}
