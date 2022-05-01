using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinBlock : MonoBehaviour
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
        }
    }
}
