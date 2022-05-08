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
            audioManager.Play(UIClipName.Accident);
            inGameMenuController.OnPlayerDeath();
        }
        if (colInfo.gameObject.layer == 4)
        {
            audioManager.Play(UIClipName.Drowned);
            inGameMenuController.OnPlayerDeath();
        }
    }
}