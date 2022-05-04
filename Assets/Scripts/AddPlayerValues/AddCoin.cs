using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : AddPlayerValues
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "Coin 5":
                    audioManager.Play(UIClipName.Coin_5);
                    carController.countsOfCoins += 5;
                    break;
                case "Coin 25":
                    audioManager.Play(UIClipName.Coin_25);
                    carController.countsOfCoins += 25;
                    break;
                case "Coin 100":
                    audioManager.Play(UIClipName.Coin_100);
                    carController.countsOfCoins += 100;
                    break;
            }
            Destroy(gameObject);
        }

    }
}
