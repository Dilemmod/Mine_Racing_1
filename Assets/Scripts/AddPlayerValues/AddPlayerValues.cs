using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerValues : MonoBehaviour
{
    protected CarController carController;
    protected UIAudioManager audioManager;
    private float rotationSpeed = 100f;
    protected bool blockGameObject = false;

    private void Start()
    {
        carController = CarController.Instance;
        audioManager = UIAudioManager.Instance;
    }
    private void FixedUpdate()
    {
        if(!blockGameObject)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
