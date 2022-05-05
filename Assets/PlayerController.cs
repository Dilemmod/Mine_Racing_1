using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject car;
    private new GameObject camera;
    private void Start()
    {
        camera = Resources.Load<GameObject>("Prefabs/Cameras/Main_Camera_3D");
        car = Resources.Load<GameObject>("Prefabs/PlayerCarsPrefab/" + PlayerPrefs.GetString("PlayerCurrentCarName"));
        car = Instantiate(car);
        camera = Instantiate(camera);
        car.transform.position = transform.position;
        camera.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z-8);
        car.transform.SetParent(transform);
        camera.transform.SetParent(transform);
    }
}
