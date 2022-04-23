using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Range(1,50)]
    [Header("Controllers")]
    private int countPanels;
    [Range(0, 500)]
    [SerializeField] private int levelPanelOffset = 15;
    [Range(0f, 20f)]
    [SerializeField] private float snapSpeed;
    [Range(0f, 10f)]
    [SerializeField] private float scaleOffset;
    [Range(0f, 30f)]
    [SerializeField] private float scaleSpeed;

    [Header("Objects")]
    [SerializeField] private GameObject levelPanelPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private int selectedPanelID;
    private bool isScrolling;

    private GameObject[] arrayOfPanels;
    private Vector2[] arrayOfPanelsPosition;
    private Vector2[] arrayOfPanelsScale;

    private RectTransform contentRect;
    private Vector2 contentVector;

    private Animator componentAnimator;

    private void Start()
    {
        componentAnimator = GetComponent<Animator>();
        foreach (Transform child in transform) Destroy(child.gameObject);

        countPanels = SceneManager.sceneCountInBuildSettings - 1;
        contentRect = GetComponent<RectTransform>();
        arrayOfPanels = new GameObject[countPanels];
        arrayOfPanelsPosition = new Vector2[countPanels];
        arrayOfPanelsScale = new Vector2[countPanels];

        for (int i = 0; i < countPanels; i++)
        {
            arrayOfPanels[i] = Instantiate(levelPanelPrefab, transform, false);
            int sceneBildIndex = i + 1;
            string levelName = EditorBuildSettings.scenes[sceneBildIndex].path.Remove(0, 14).Replace(".unity", "");
            arrayOfPanels[i].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load("LevelImages/"+ levelName, typeof(Sprite)) as Sprite;
            arrayOfPanels[i].GetComponentInChildren<Text>().text = levelName.ToUpper();
            arrayOfPanels[i].transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text = (PlayerPrefs.HasKey(sceneBildIndex + "PlayerRecord")? PlayerPrefs.GetInt(sceneBildIndex + "PlayerRecord").ToString():"0")+"m";
            arrayOfPanels[i].GetComponent<Button>().onClick.AddListener(() => OnChangeLevelClicked(sceneBildIndex));

            if (i == 0) continue;
            //coordinates of the last panel + panel width + padding
            arrayOfPanels[i].transform.localPosition = new Vector2(
                arrayOfPanels[i - 1].transform.localPosition.x + levelPanelPrefab.GetComponent<RectTransform>().sizeDelta.x + levelPanelOffset,
                arrayOfPanels[i].transform.localPosition.y);
            arrayOfPanelsPosition[i] = -arrayOfPanels[i].transform.localPosition;
        }
    }
    private void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= arrayOfPanelsPosition[0].x && !isScrolling ||
           contentRect.anchoredPosition.x <= arrayOfPanelsPosition[arrayOfPanelsPosition.Length - 1].x && isScrolling)  
        {
            scrollRect.inertia = false;
        }
        float nearestPosition = float.MaxValue;
        for (int i = 0; i < countPanels; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - arrayOfPanelsPosition[i].x);
            if (distance < nearestPosition)
            {
                nearestPosition = distance;
                selectedPanelID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / levelPanelOffset) * scaleOffset, 0.5f, 1);
            arrayOfPanelsScale[i].x = Mathf.SmoothStep(arrayOfPanels[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            arrayOfPanelsScale[i].y = Mathf.SmoothStep(arrayOfPanels[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            arrayOfPanels[i].transform.localScale = arrayOfPanelsScale[i];
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling)  scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        if (isScrolling) return;
        contentVector.x = Mathf.SmoothStep(
            contentRect.anchoredPosition.x, 
            arrayOfPanelsPosition[selectedPanelID].x,
            snapSpeed*Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }
    public void OnChangeLevelClicked(int sceneBildIndex)
    {
        SceneTransition.SwitchToScene(sceneBildIndex);
    }
    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
}
