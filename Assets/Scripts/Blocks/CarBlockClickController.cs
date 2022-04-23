using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBlockClickController : MonoBehaviour
{
    private CarController carController;
    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            carController = CarController.Instance;

            switch (gameObject.tag)
            {
                case "Block 25":
                    carController.countsOfCoins += 25;
                    break;
                case "Block 100":
                    carController.countsOfCoins += 100;
                    break;
                case "Block 500":
                    carController.countsOfCoins += 500;
                    break;
            }
            Destroy(this.transform.parent.gameObject);
        }/*
        if (Input.GetButtonDown("Fire2"))
        {
            newPosition = this.transform.parent.transform.position + delta;
            // Клон
            GameObject clone = Instantiate(usedBlock, newPosition, Quaternion.identity);
            // Позиция
            clone.transform.position = newPosition;
            // Переименовывем
            clone.name = "Block@" + clone.transform.position;
        }*/
    }
}
