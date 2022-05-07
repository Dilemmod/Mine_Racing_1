using System;
using UnityEngine;
using UnityEngine.UI;

public class TuningMenu : MonoBehaviour
{
    //-70
    [Header("Cars")]
    [SerializeField] private GameObject playerCars;

    [Header("Buttons")]
    [SerializeField] private Button unlock;
    private Text priceText;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    [Header("Sliders")]
    [SerializeField] private Text nameCarText_1;
    [SerializeField] private Text nameCarText_2;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider fuelEfficiencySlider;
    [SerializeField] private Slider gravitySlider;
    [SerializeField] private Slider motorsCountSlider;

    private UIAudioManager audioManager;
    private GameObject currentCar;
    private int usedCarID = 0;
    #region Set Value from PlayerPrefs
    private void Start()
    {
        audioManager = UIAudioManager.Instance;
        priceText = unlock.transform.GetChild(0).GetComponentInChildren<Text>();
        //Current Player Car
        PlayerPrefs.SetString("PlayerCurrentCarName",
            PlayerPrefs.HasKey("PlayerCurrentCarName") ?
            PlayerPrefs.GetString("PlayerCurrentCarName") : "Jeep");

        ActivateCarByName(PlayerPrefs.GetString("PlayerCurrentCarName"));
    }
    private void OnLeftArrowClicked()
    {
        audioManager.Play(UIClipName.Play);
        usedCarID -= 1;
        if (usedCarID < 0) usedCarID = playerCars.transform.childCount - 1;
            ActivateCarByID(usedCarID);
    }

    private void OnRightArrowClicked()
    {
        audioManager.Play(UIClipName.Quit);
        usedCarID += 1;
        if (usedCarID > playerCars.transform.childCount - 1) 
            usedCarID = 0;
            ActivateCarByID(usedCarID);
    }
    private void ActivateCarByName(string name)
    {
        foreach (Transform child in playerCars.transform)
        {
            if (child.name == name)
                currentCar = child.gameObject;
            child.gameObject.SetActive(false);
        }
        SetSlidersToDefault();
    }
    private void ActivateCarByID(int ID)
    {
        foreach (Transform child in playerCars.transform)
            child.gameObject.SetActive(false);
        currentCar = playerCars.transform.GetChild(ID).gameObject;
        SetSlidersToDefault();
    }
    private void SetSlidersToDefault()
    {
        nameCarText_1.text = currentCar.name.ToUpper();
        nameCarText_2.text = currentCar.name.ToUpper();
        currentCar.gameObject.SetActive(true);
        //Set Player Prefs Values
        speedSlider.value = PlayerPrefs.GetFloat(currentCar.name + "speed");
        fuelEfficiencySlider.value = PlayerPrefs.GetFloat(currentCar.name + "fuelEfficiency");
        gravitySlider.value = PlayerPrefs.GetFloat(currentCar.name + "gravity");
        motorsCountSlider.value = (int)PlayerPrefs.GetFloat(currentCar.name + "motorCount");
    }
    private void SetSlidersToDefault(Slider slider)
    {
        if(slider != speedSlider)
            speedSlider.value = PlayerPrefs.GetFloat(currentCar.name + "speed");
        if (slider != fuelEfficiencySlider)
            fuelEfficiencySlider.value = PlayerPrefs.GetFloat(currentCar.name + "fuelEfficiency");
        if (slider != gravitySlider)
            gravitySlider.value = PlayerPrefs.GetFloat(currentCar.name + "gravity");
        if (slider != motorsCountSlider)
            motorsCountSlider.value = (int)PlayerPrefs.GetFloat(currentCar.name + "motorCount");
    }
    private void SetTextValueInSlider(Slider slider)
    {
        slider.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
            Math.Floor(slider.value).ToString();
    }
    #endregion
    private void OnUnlockClicked()
    {
        int plyerCoins = PlayerPrefs.GetInt("PlayerCoins");
        int currentPrice = int.Parse(priceText.text);
        if (plyerCoins > currentPrice)
        {
            PlayerPrefs.SetInt("PlayerCoins", plyerCoins - currentPrice);
            audioManager.Play(UIClipName.Buy);
            SetNewValuesToPlayerPrefs();
        }
        else
        {
            audioManager.Play(UIClipName.Fail);
        }
        //SetPriceChanged(2, 2);
        PlayerPrefs.SetString("PlayerCurrentCarName", currentCar.name);
        Debug.Log(PlayerPrefs.GetString("PlayerCurrentCarName"));
        //PlayerPrefs.SetString("PlayerCurrentCarName", GameObject.FindGameObjectWithTag("Player").name);
    }
    private void SetNewValuesToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(currentCar.name + "speed", (float)speedSlider.value);
        PlayerPrefs.SetFloat(currentCar.name + "fuelEfficiency", (float)fuelEfficiencySlider.value);
        PlayerPrefs.SetFloat(currentCar.name + "gravity", (float)gravitySlider.value);
        PlayerPrefs.SetFloat(currentCar.name + "motorCount", (int)motorsCountSlider.value);
    }
    private void SetPriceChanged(float value, float priceGrowth, string parameter)
    {
        double price = 0;
        float currentValue =
            PlayerPrefs.HasKey(currentCar.name + parameter) ?
            PlayerPrefs.GetFloat(currentCar.name + parameter) : 0;
        if (value > currentValue)
        {
            price =+ Math.Floor((value- currentValue) * priceGrowth);
        }else if(value < currentValue)
        {
            price =+Math.Floor((currentValue-value) * (priceGrowth/2));
        }
        priceText.text = price.ToString();
    }
    private void OnFuelEfficiencySliderValueChanged(float value)
    {
        SetSlidersToDefault(fuelEfficiencySlider);
        SetTextValueInSlider(fuelEfficiencySlider);
        SetPriceChanged(value, 100, "fuelEfficiency");
    }

    private void OnSpeedValueChanged(float value)
    {
        SetSlidersToDefault(speedSlider);
        SetTextValueInSlider(speedSlider);
        SetPriceChanged(value,10,"speed");
    }

    private void OnMotorsCountSliderValueChanged(float value)
    {
        SetSlidersToDefault(motorsCountSlider);
        SetTextValueInSlider(motorsCountSlider);
        SetPriceChanged(value, 300, "motorCount");
    }

    private void OnGravitySliderValueChanged(float value)
    {
        SetSlidersToDefault(gravitySlider);
        SetTextValueInSlider(gravitySlider);
        SetPriceChanged(value, 50,"gravity");
    }

    private void Awake()
    {
        leftArrow.onClick.AddListener(OnLeftArrowClicked);
        rightArrow.onClick.AddListener(OnRightArrowClicked);
        unlock.onClick.AddListener(OnUnlockClicked);

        speedSlider.onValueChanged.AddListener(OnSpeedValueChanged);
        fuelEfficiencySlider.onValueChanged.AddListener(OnFuelEfficiencySliderValueChanged);
        gravitySlider.onValueChanged.AddListener(OnGravitySliderValueChanged);
        motorsCountSlider.onValueChanged.AddListener(OnMotorsCountSliderValueChanged);
    }
    protected void OnDestroy()
    {
        speedSlider.onValueChanged.RemoveListener(OnSpeedValueChanged);
        fuelEfficiencySlider.onValueChanged.RemoveListener(OnFuelEfficiencySliderValueChanged);
        gravitySlider.onValueChanged.RemoveListener(OnGravitySliderValueChanged);
        motorsCountSlider.onValueChanged.RemoveListener(OnMotorsCountSliderValueChanged);

        leftArrow.onClick.RemoveListener(OnLeftArrowClicked);
        rightArrow.onClick.RemoveListener(OnRightArrowClicked);
        unlock.onClick.RemoveListener(OnUnlockClicked);
    }






}
