using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Components")]
    //[SerializeField] private Text textX;
    //[SerializeField] private Text textY;
   // [SerializeField] private Text textZ;
    [SerializeField] private InGameMenuController inGameMenuController;
    [SerializeField] private WheelJoint2D backWheel;
    [SerializeField] private WheelJoint2D frontWheel;
    //[SerializeField] private Rigidbody2D rbBackWheel;
    //[SerializeField] private Rigidbody2D rbFrontWheel;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private UIButtonInfo gasButton;
    [SerializeField] private UIButtonInfo breakButton;
    [SerializeField] private GameObject ObjectBackWheel;
    [SerializeField] private GameObject ObjectFrontWheel;
    [NonSerialized] private Collider2D backWheelCollider2D;
    [NonSerialized] private Collider2D frontWheelCollider2D;
    //[SerializeField] private Collider2D roadCollider2D;
     static private float startCarPositionX;

    [Header("Movement")]
    [SerializeField] private float speed = 1500f;
    [SerializeField] private float rotationSpeed = 10f;
    [NonSerialized] public float movement = 0f;
    [NonSerialized] public float direction = 0f;
    [SerializeField]private bool isGrounded;
    // Vector3 acceleration;
    //private bool moveUp = false;
    // private bool moveDown = false;
    // private bool grounded = false;

    [Header("Fuel")]
    [SerializeField] private Text textFuelValue;
    [SerializeField] private Slider sliderFuel;
    [NonSerialized] public float fuel;
    [SerializeField] public float maxFuel = 100f;
    [SerializeField] private float fuelconsumption = 1f;

    [Header("Coins")]
    [SerializeField] private Text textCoinsValue;
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
        backWheelCollider2D = ObjectBackWheel.gameObject.GetComponent<Collider2D>();
        frontWheelCollider2D = ObjectFrontWheel.gameObject.GetComponent<Collider2D>();
        startCountOfCoins = countsOfCoins;
        startCarPositionX = rb.position.x;
        countsOfCoins = (PlayerPrefs.HasKey("PlayerCoins") ? PlayerPrefs.GetInt("PlayerCoins") : 0);
        //backWheelCollider2D = backWheel.gameObject.GetComponent<Collider2D>();
        //frontWheelCollider2D = frontWheel.gameObject.GetComponent<Collider2D>();
        fuel = maxFuel;
        sliderFuel.maxValue = fuel;
        sliderFuel.value = fuel;
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
        if (gasButton.isDown|| Input.GetAxisRaw("Horizontal")==1)
            direction = -1f ;
        else if (breakButton.isDown|| Input.GetAxisRaw("Horizontal") == -1)
            direction = 1f ;
        else
            direction = 0f;
        movement = direction * speed;
        if (fuel > 0f)
        {
            //rbBackWheel.AddTorque(movement * Time.fixedDeltaTime);
            //rbFrontWheel.AddTorque(movement * Time.fixedDeltaTime);
            //rb.AddTorque(movement * Time.fixedDeltaTime);
            //Motor
            
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
                    rb.AddTorque(-direction * rotationSpeed * Time.fixedDeltaTime*100f);
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
        PlayerPrefs.SetInt("PlayerCoins",  (startCountOfCoins+ countsOfCoins));
        inGameMenuController.OnPlayerDeath();
    }/*
    public void OnWin()
    {
        inGameMenuController.OnPlayerWin();
    }*/
}