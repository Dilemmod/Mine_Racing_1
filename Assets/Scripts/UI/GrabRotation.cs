using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private ForceMode forceMode;
    private float X_axisRotation;
    bool inertia = false;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (inertia)
        {
            X_axisRotation *=  -Time.deltaTime;
            rb.AddTorque(transform.up * X_axisRotation, forceMode);
        }
    }
    void OnMouseDrag()
    {
        X_axisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        rb.AddTorque(transform.up * -X_axisRotation, forceMode);
    }
    void OnMouseDown()
    {
        inertia = false;
    }
    void OnMouseUp()
    {
        inertia = true;
    }

}

