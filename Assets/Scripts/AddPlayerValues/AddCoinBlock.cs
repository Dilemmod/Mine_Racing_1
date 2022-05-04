using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinBlock : AddPlayerValues
{
    private void Awake()
    {
        blockGameObject = true;
    }
    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            switch (gameObject.tag)
            {
                case "Block 25":
                    audioManager.Play(UIClipName.Block_25);
                    carController.countsOfCoins += 25;
                    break;
                case "Block 100":
                    audioManager.Play(UIClipName.Block_100);
                    carController.countsOfCoins += 100;
                    break;
                case "Block 500":
                    audioManager.Play(UIClipName.Block_500);
                    carController.countsOfCoins += 500;
                    break;
            }
            Destroy(this.transform.parent.gameObject);
        }
    }
}
