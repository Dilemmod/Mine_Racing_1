using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : MonoBehaviour
{
    private CarController carController;
    private float rotationSpeed = 100f;
    private void Awake()
    {
        carController = CarController.Instance;
        //carController = GameObject.Find("Car").GetComponent<CarController>();
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        carController.fuel = carController.maxFuel;
        Destroy(gameObject);
        
    }
}
