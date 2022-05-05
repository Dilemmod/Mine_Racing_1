using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform Target;
    private GameObject PlayerFollowers;
    [SerializeField] private float SmoothSpeed = 0.125f;
    [SerializeField] private Vector3 Offset;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerFollowers = GameObject.FindGameObjectWithTag("Player_Followers");
    }
    private void FixedUpdate()
    {
        Vector3 desiredPosition = Target.position + Offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
        transform.position = smoothPosition;
        //transform.LookAt(target);
        if(PlayerFollowers != null)
            PlayerFollowers.transform.position = new Vector3(smoothPosition.x, PlayerFollowers.transform.position.y);
    }
}
