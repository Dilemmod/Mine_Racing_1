// CHANGE LOG
// 
// CHANGES || version VERSION
//
// "Enable/Disable Headbob, Changed look rotations - should result in reduced camera jitters" || version 1.0.1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.Net;
#endif

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;

    #endregion

    #region Movement Variables

    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    #region Jump

    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    #endregion

    #region Crouch

    public KeyCode crouchKey = KeyCode.LeftControl;

    #endregion
    #endregion
    #region Singleton
    public static FirstPersonController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        //Awake
        rb = GetComponent<Rigidbody>();
        crosshairObject = GetComponentInChildren<Image>();
        // Set internal variables
        playerCamera.fieldOfView = fov;


        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }

    }

    float camRotation;

    private void Update()
    {
        #region Camera

        // Control camera movement
        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                // Inverted Y
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        #endregion

        #region Up

        // Up
        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        #endregion
        #region Down

        // Down
        if (Input.GetKeyDown(crouchKey))
        {
            Crouch();
        }

        #endregion
    }

    void FixedUpdate()
    {
        #region Movement

        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        // All movement calculations while walking


        targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);



        #endregion
    }

    private void Jump()
    {
        Vector3 posPlayer = new Vector3(rb.position.x, rb.position.y + jumpPower, rb.position.z);
        rb.MovePosition(posPlayer);
    }

    private void Crouch()
    {
        Vector3 posPlayer = new Vector3(rb.position.x, rb.position.y - jumpPower, rb.position.z);
        rb.MovePosition(posPlayer);

    }


}
