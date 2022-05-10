using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    private InGameMenuController inGameMenuController;
    private UIAudioManager audioManager;
    private void Start()
    {
        inGameMenuController = InGameMenuController.Instance;
        audioManager = UIAudioManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D colInfo)
    {
        if (colInfo.gameObject.layer==11)
        {
            inGameMenuController.gameOverHeader.text = "CRASHED";
            audioManager.Play(UIClipName.Accident);
            inGameMenuController.OnPlayerDeath();
        }
        if (colInfo.gameObject.layer == 4)
        {
            inGameMenuController.gameOverHeader.text = "DEOWNED";
            inGameMenuController.gameOverHeader.color = new Color(118, 255, 255);
            audioManager.Play(UIClipName.Drowned);
            inGameMenuController.OnPlayerDeath();
        }
    }
}