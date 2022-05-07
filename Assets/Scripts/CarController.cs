using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private float PitchRiseRate = 0.01f;
    [SerializeField] private float PitchSinkRate = 0.05f;
    private UIAudioManager audioManager;
    private AudioSource CarSound;
    private float pitch = 0.5f;
    [Header("Components")]
    private InGameMenuController inGameMenuController;
    private new ParticleSystem particleSystem;
    private Rigidbody2D rb;
    private UIButtonInfo gasButton;
    private UIButtonInfo breakButton;
    private Slider sliderFuel;
    private Text textFuelValue;
    private Text textCoinsValue;
    static private float startCarPositionX;
    private Collider2D backWheelCollider2D;
    private Collider2D frontWheelCollider2D;
    private WheelJoint2D[] wheelJoints2D;
    private List<GameObject> wheelGameObjects = new List<GameObject>();
    //private WheelJoint2D wheelBackJoint2D;
    private GameObject wheelFrontGemeObject; 
    private GameObject wheelBackGemeObject;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 20;
    [NonSerialized] public float movement = 0f;
    [NonSerialized] public float direction = 0f;
    private bool isGrounded;

    [Header("Fuel")]
    [NonSerialized] public float fuel;
    [SerializeField] public float maxFuel = 100f;

    [Header("Coins")]
    [SerializeField] public int countsOfCoins;
    static private int startCountOfCoins;

    //Player customize
    private float speed = 1200;
    private float fuelEfficiency = 4f;
    private float frontWheelGravity = 5;
    private float backWheelGravity = 1;
    private int motorCount = 1;


    #region Singleton
    public static CarController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion
    private void Start()
    {
        //Player customize
        speed = 
            PlayerPrefs.HasKey(transform.name + "speed") ? 
            PlayerPrefs.GetFloat(transform.name + "speed")*50 : speed;
        fuelEfficiency = 
            PlayerPrefs.HasKey(transform.name + "fuelEfficiency") ? 
            PlayerPrefs.GetFloat(transform.name + "fuelEfficiency") : fuelEfficiency;
        frontWheelGravity = 
            PlayerPrefs.HasKey(transform.name + "gravity") ?
            PlayerPrefs.GetFloat(transform.name + "gravity") : frontWheelGravity;
        motorCount =
            PlayerPrefs.HasKey(transform.name + "motorCount") ?
            (int)PlayerPrefs.GetFloat(transform.name + "motorCount") : motorCount;

        //Game menu
        audioManager = UIAudioManager.Instance;
        inGameMenuController = InGameMenuController.Instance;
        if (inGameMenuController == null || audioManager == null)
        {
            Debug.LogError("Add Canvas with audioManager and inGameMenuController");
            return;
        }

        //Wheels objects
        wheelJoints2D = GetComponents<WheelJoint2D>();
        foreach (Transform Child in gameObject.transform)
        {
            if (Child.gameObject.layer == 9)
                wheelGameObjects.Add(Child.gameObject);
        }
        //Wheels mass
        wheelGameObjects[0].GetComponent<Rigidbody2D>().mass = frontWheelGravity;
        wheelGameObjects[wheelGameObjects.Count-1].GetComponent<Rigidbody2D>().mass = backWheelGravity;

        //Wheels assignment
        wheelFrontGemeObject = wheelGameObjects[0];
        wheelBackGemeObject = wheelGameObjects[wheelGameObjects.Count-1];


        //Get Objects
        particleSystem =  GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        gasButton = GameObject.Find("Canvas/GasButton").GetComponent<UIButtonInfo>() ;
        breakButton = GameObject.Find("Canvas/BreakButton").GetComponent<UIButtonInfo>();
        textCoinsValue = GameObject.Find("Canvas/Bars/Coin/CoinText").GetComponent<Text>();
        textFuelValue = GameObject.Find("Canvas/Bars/FuelSlider/FuelText").GetComponent<Text>();
        sliderFuel = GameObject.Find("Canvas/Bars/FuelSlider").GetComponent<Slider>();

        startCarPositionX = rb.position.x;
        backWheelCollider2D = wheelBackGemeObject.gameObject.GetComponent<Collider2D>();
        frontWheelCollider2D = wheelFrontGemeObject.gameObject.GetComponent<Collider2D>();
        countsOfCoins = (PlayerPrefs.HasKey("PlayerCoins") ? PlayerPrefs.GetInt("PlayerCoins") : 0);
        startCountOfCoins = countsOfCoins;
        //Fuel
        fuel = maxFuel;
        sliderFuel.maxValue = fuel;
        sliderFuel.value = fuel;
        //7 == EngineSound
        CarSound = audioManager.GetComponents<AudioSource>()[7];
        CarSound.Play();
    }
    private void Update()
    {
        if (frontWheelCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))
            || backWheelCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            isGrounded = true;
        else
            isGrounded = false;
    }

    [Obsolete]
    private void FixedUpdate()
    {
        if (gasButton.isDown || Input.GetAxisRaw("Horizontal") == 1)
        {
            direction = -1f;
        }
        else if (breakButton.isDown || Input.GetAxisRaw("Horizontal") == -1)
            direction = 1f;
        else
            direction = 0f;
        movement = direction * speed;
        if (fuel > 0f)
        {
            if (movement == 0f)
            {
                for (int i = 0; i < motorCount; i++)
                {
                    wheelJoints2D[i].useMotor = false;
                }
            }
            else
            {
                if (isGrounded)
                {
                    JointMotor2D motor = new JointMotor2D 
                    { motorSpeed = movement, maxMotorTorque = wheelJoints2D[wheelJoints2D.Length - 1].motor.maxMotorTorque };
                    for (int i = 0; i < motorCount; i++)
                    {
                        wheelJoints2D[i].motor = motor;
                    }
                }
                else
                {
                    rb.AddTorque(-direction * rotationSpeed * Time.fixedDeltaTime * 100f);
                }
            }
            //rb.AddTorque(-acceleration.x * rotationSpeed );
            //rb.AddTorque(rotation * rotationSpeed * Time.fixedDeltaTime);

        }
        if (movement != 0f)
            fuel -= fuelEfficiency * Time.fixedDeltaTime;
        else
            fuel -= (fuelEfficiency / 5) * Time.fixedDeltaTime;
        if (fuel <= 0f)
            OnDeath();
        textCoinsValue.text = countsOfCoins.ToString();
        textFuelValue.text = Math.Ceiling(fuel).ToString();
        sliderFuel.value = fuel;

        //Effects
        particleSystem.playbackSpeed = 2;
        if (direction == -1)
        {
            if (isGrounded && particleSystem.isStopped)
                particleSystem.Play();
            if(pitch <= 2.5)
                pitch += PitchRiseRate;
        }
        else
        {
            if ((!isGrounded || particleSystem.isPlaying))
                particleSystem.Stop();
            if(pitch >= 0.5)
                pitch -= PitchSinkRate;
        }
        CarSound.pitch = pitch;
    }
    public Vector3 GetPlayerPosition()
    {
        try
        {
            return rb.gameObject.transform.position;
        }
        catch 
        {
            return new Vector3(0, 0, 0);
        }
        
    }
    public int GetPlayerTravelDistance()
    {
        int playerTravelDistance = (int)Math.Round(rb.transform.position.x - startCarPositionX);
        playerTravelDistance = (playerTravelDistance > 0 ? playerTravelDistance : 0);
        return playerTravelDistance;
    }
    public void OnDeath()
    {
        inGameMenuController.DistanceValue.text = GetPlayerTravelDistance().ToString();
        inGameMenuController.CoinsValue.text = (countsOfCoins - startCountOfCoins).ToString();
        PlayerPrefs.SetInt("PlayerCoins", countsOfCoins);
        inGameMenuController.OnPlayerDeath();
    }
}