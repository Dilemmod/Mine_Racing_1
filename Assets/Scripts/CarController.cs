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
    [SerializeField] private WheelJoint2D backWheel;
    [SerializeField] private WheelJoint2D frontWheel;
    [SerializeField] private GameObject ObjectBackWheel;
    [SerializeField] private GameObject ObjectFrontWheel;

    [Header("Movement")]
    [SerializeField] private float speed = 1500f;
    [SerializeField] private float rotationSpeed = 10f;
    [NonSerialized] public float movement = 0f;
    [NonSerialized] public float direction = 0f;
    [SerializeField] private bool isGrounded;

    [Header("Fuel")]
    [NonSerialized] public float fuel;
    [SerializeField] public float maxFuel = 100f;
    [SerializeField] private float fuelconsumption = 1f;

    [Header("Coins")]
    [SerializeField] public int countsOfCoins;
    static private int startCountOfCoins;

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

        //Game menu
        audioManager = UIAudioManager.Instance;
        inGameMenuController = InGameMenuController.Instance;
        if (inGameMenuController == null|| audioManager==null)
        {
            Debug.LogError("Add Canvas with audioManager and inGameMenuController");
            return;
        }
        //Get Objects
        particleSystem =  GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        gasButton = GameObject.Find("Canvas/GasButton").GetComponent<UIButtonInfo>() ;
        breakButton = GameObject.Find("Canvas/BreakButton").GetComponent<UIButtonInfo>();
        textCoinsValue = GameObject.Find("Canvas/Bars/Coin/CoinText").GetComponent<Text>();
        textFuelValue = GameObject.Find("Canvas/Bars/FuelSlider/FuelText").GetComponent<Text>();
        sliderFuel = GameObject.Find("Canvas/Bars/FuelSlider").GetComponent<Slider>();

        startCarPositionX = rb.position.x;
        backWheelCollider2D = ObjectBackWheel.gameObject.GetComponent<Collider2D>();
        frontWheelCollider2D = ObjectFrontWheel.gameObject.GetComponent<Collider2D>();
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
                backWheel.useMotor = false;
                frontWheel.useMotor = false;
            }
            else
            {
                if (isGrounded)
                {
                    JointMotor2D motor = new JointMotor2D();
                    motor = new JointMotor2D { motorSpeed = movement, maxMotorTorque = backWheel.motor.maxMotorTorque };
                    //frontWheel.motor = motor;
                    backWheel.motor = motor;
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
            fuel -= fuelconsumption * Time.fixedDeltaTime;
        else
            fuel -= (fuelconsumption / 5) * Time.fixedDeltaTime;
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
    /*
    private void Update()
    {
        movement = -Input.GetAxisRaw("Horizontal") * speed;
        if(!grounded)
            rotation = Input.GetAxisRaw("Horizontal");
        textCoinsValue.text = countsOfCoins.ToString();
    }
    *//*
    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
        Debug.Log(other.gameObject.name);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false ;
        Debug.Log(isGrounded);
    }*/
    public Vector3 GetPlayerPosition()
    {
        if (rb.gameObject != null)
            return rb.gameObject.transform.position;
        else
            return new Vector3(0,0,0);
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