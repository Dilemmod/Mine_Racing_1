using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float SmoothSpeed = 0.125f;
    [SerializeField] private Vector3 Offset;

    [SerializeField] private GameObject PlayerFollower;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = Target.position + Offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
        transform.position = smoothPosition;
        //transform.LookAt(target);
        if(PlayerFollower!=null)
            PlayerFollower.transform.position = new Vector3(smoothPosition.x, PlayerFollower.transform.position.y);
    }
}
