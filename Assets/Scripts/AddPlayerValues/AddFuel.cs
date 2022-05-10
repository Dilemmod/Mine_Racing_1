using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : AddPlayerValues
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioManager.Play(UIClipName.Fuel);
        carController.fuel = Random.Range(carController.fuel, carController.maxFuel);
        Destroy(gameObject);
    }
}
