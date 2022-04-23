using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : MonoBehaviour
{
    private CarController carController;
    private float rotationSpeed = 100f;

    private void Start()
    {
        carController = CarController.Instance;
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            switch (gameObject.tag)
            {
                case "Coin 5":
                    carController.countsOfCoins += 5;
                    break;
                case "Coin 25":
                    carController.countsOfCoins += 25;
                    break;
                case "Coin 100":
                    carController.countsOfCoins += 100;
                    break;
            }
            Destroy(gameObject);
        }

    }
}
