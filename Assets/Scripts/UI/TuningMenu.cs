using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TuningMenu : MonoBehaviour
{
    //-70
    [Header("Cars")]
    [SerializeField] private GameObject playerCars;

    [Header("Buttons")]
    [SerializeField] private Button unlock;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    private UIAudioManager audioManager;
    //[SerializeField] private GameObject[] cars;
    //временно
    private int usedCarID = 0;
    private void Awake()
    {
        leftArrow.onClick.AddListener(OnLeftArrowClicked);
        rightArrow.onClick.AddListener(OnRightArrowClicked);
        unlock.onClick.AddListener(OnUnlockClicked);
    }
    protected void OnDestroy()
    {
        leftArrow.onClick.RemoveListener(OnLeftArrowClicked);
        rightArrow.onClick.RemoveListener(OnRightArrowClicked);
        unlock.onClick.RemoveListener(OnUnlockClicked);
    }
    private void Start()
    {
        ActivateCarByID(usedCarID);
        /*
        cars = new GameObject[playerCars.transform.childCount];
        for (int i = 0; i < cars.Length; i++)
        {
            //cars[i] = playerCars.transform.GetChild(i).gameObject;
        }
        AddCarByID(usedCarID);*/
    }
    private void ActivateCarByID(int ID)
    {
        foreach (Transform child in playerCars.transform) child.gameObject.SetActive(false);
        playerCars.transform.GetChild(ID).gameObject.SetActive(true);
    }
    private void AddCarByID(int ID)
    {
        //foreach (Transform child in playerCars.transform) Destroy(child.gameObject);
        //Instantiate(cars[ID], playerCars.transform, false);
    }
    private void OnLeftArrowClicked()
    {
        usedCarID -= 1;
        //if (usedCarID < 0) usedCarID = cars.Length - 1;
        if (usedCarID < 0) usedCarID = playerCars.transform.childCount-1 ;
        ActivateCarByID(usedCarID);
    }
    private void OnRightArrowClicked()
    {
        usedCarID += 1;
        if (usedCarID > playerCars.transform.childCount -1) usedCarID = 0;
        ActivateCarByID(usedCarID);
    }
    private void OnUnlockClicked()
    {
        PlayerPrefs.SetString("PlayerCurrentCarName", GameObject.FindGameObjectWithTag("Player").name);
    }






}
