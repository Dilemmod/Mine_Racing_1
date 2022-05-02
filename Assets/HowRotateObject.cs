using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowRotateObject : MonoBehaviour
{
    [SerializeField] private float[] Rotation;
    public float GetRandomRotate()
    {
        if (Rotation.Length == 0)
            return 0;
        else
            return Rotation[Random.Range(0, Rotation.Length)];
    }
}
