using System;
using UnityEngine;
using UnityEngine.UI;

public class TuningMenu : MonoBehaviour
{
    [Header("Cars")]
    [SerializeField] private GameObject playerCars;

    [Header("Buttons")]
    [SerializeField] private Button unlock;
    private Text priceText;
    private Text priceNameText;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    [Header("Sliders")]
    [SerializeField] private Text nameCarText_1;
    [SerializeField] private Text nameCarText_2;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider fuelEfficiencySlider;
    [SerializeField] private Slider gravitySlider;
    [SerializeField] private Slider motorsCountSlider;
    //private Image NotPurchasedCarsImage;

    private UIAudioManager audioManager;
    private GameObject currentCar;
    private int usedCarID = 0;
    #region Set Value from PlayerPrefs
    private void Start()
    {
        audioManager = UIAudioManager.Instance;
        //NotPurchasedCarsImage = transform.GetChild(1).GetComponent<Image>();
        priceText = unlock.transform.GetChild(0).GetComponentInChildren<Text>();
        priceNameText = unlock.transform.GetChild(2).GetComponent<Text>();
        //Current Player Car
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
        for (int i = 0; i < playerCars.transform.childCount; i++)
        {
            if (playerCars.transform.GetChild(i).name == name)
            {
                usedCarID = i;
                currentCar = playerCars.transform.GetChild(i).gameObject;
            }
            playerCars.transform.GetChild(i).gameObject.SetActive(false);
        }
        SetSlidersToDefault();
        SwitchPlayerBoughtCarMenu();
    }
    private void ActivateCarByID(int ID)
    {
        foreach (Transform child in playerCars.transform)
            child.gameObject.SetActive(false);
        currentCar = playerCars.transform.GetChild(ID).gameObject;
        SetSlidersToDefault();
        SwitchPlayerBoughtCarMenu();
    }
    private void SwitchPriceText(StateCarPrice state)
    {
        switch (state)
        {
            case StateCarPrice.SelectCar:
                currentStateCarPrice = StateCarPrice.SelectCar;
                transform.GetChild(1).gameObject.SetActive(false);
                priceNameText.text = "CAR";
                priceText.text = "SELECT";
                break;
            case StateCarPrice.Upgrade:
                currentStateCarPrice = StateCarPrice.Upgrade;
                transform.GetChild(1).gameObject.SetActive(false);
                priceNameText.text = "UP PRICE";

                break;
            case StateCarPrice.BuyCar:
                currentStateCarPrice = StateCarPrice.BuyCar;
                transform.GetChild(1).gameObject.SetActive(true);
                priceNameText.text = "CAR PRICE";
                priceText.text = PlayerPrefs.GetFloat(currentCar.name + "carPrice").ToString();
                break;
        }
    }
    private StateCarPrice currentStateCarPrice { get; set; }
    enum StateCarPrice
    {
        SelectCar,
        Upgrade,
        BuyCar
    }
    private void SwitchPlayerBoughtCarMenu()
    {
        if (PlayerPrefs.GetFloat(currentCar.name + "playerBoughtCar") == 1f)
        {
            SwitchPriceText(StateCarPrice.SelectCar);
        }
        else
        {
            SwitchPriceText(StateCarPrice.BuyCar);
        }
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
        // Set visualized text
        SetTextValueInSlider(speedSlider);
        SetTextValueInSlider(fuelEfficiencySlider);
        SetTextValueInSlider(gravitySlider);
        SetTextValueInSlider(motorsCountSlider);
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
        switch (currentStateCarPrice)
        {
            case StateCarPrice.SelectCar:
                PlayerPrefs.SetString("PlayerCurrentCarName", currentCar.name);
                audioManager.Play(UIClipName.Select);
                break;
            case StateCarPrice.Upgrade:
            case StateCarPrice.BuyCar:
                int currentPrice = int.Parse(priceText.text);
                if (plyerCoins > currentPrice)
                {
                    //Upgrade Car
                    PlayerPrefs.SetInt("PlayerCoins", plyerCoins - currentPrice);
                    SetNewValuesToPlayerPrefs();

                    //BuyCar
                    PlayerPrefs.SetFloat(currentCar.name + "playerBoughtCar", 1f);
                    PlayerPrefs.SetString("PlayerCurrentCarName", currentCar.name);
                    SwitchPriceText(StateCarPrice.SelectCar);

                    //Set Player coins text
                    MainMenuController.Instance.playerCoinsText.text =
                        PlayerPrefs.GetInt("PlayerCoins").ToString();
                    //Audio
                    audioManager.Play(UIClipName.Buy);
                }
                else
                {
                    audioManager.Play(UIClipName.Fail);
                }
                break;
        }
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
    private void OnSliderValueChanged(Slider slider,float value, float priceGrowth,string parameter) 
    {
        SwitchPriceText(StateCarPrice.Upgrade);
        SetSlidersToDefault(slider);
        SetTextValueInSlider(slider);
        SetPriceChanged(value, priceGrowth, parameter);
    }
    private void OnFuelEfficiencySliderValueChanged(float value)
    {
        OnSliderValueChanged(fuelEfficiencySlider, value, 400, "fuelEfficiency");
    }

    private void OnSpeedValueChanged(float value)
    {
        OnSliderValueChanged(speedSlider, value, 20, "speed");
    }

    private void OnMotorsCountSliderValueChanged(float value)
    {
        OnSliderValueChanged(motorsCountSlider, value, 500, "motorCount");
    }

    private void OnGravitySliderValueChanged(float value)
    {
        OnSliderValueChanged(gravitySlider, value, 50, "gravity");
    }






}
