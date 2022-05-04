using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    private CarController carController;
    private UIAudioManager audioManager;
    private void Start()
    {
        carController = CarController.Instance;
        audioManager = UIAudioManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D colInfo)
    {
        if (colInfo.gameObject.layer==11)
        {
            audioManager.Play(UIClipName.Accident);
            carController.OnDeath();
        }
        if (colInfo.gameObject.layer == 4)
        {
            audioManager.Play(UIClipName.Drowned);
            carController.OnDeath();
        }
    }
}