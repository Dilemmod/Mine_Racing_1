using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarValue : MonoBehaviour
{
    [NonSerialized] public string carName;
    [SerializeField] public bool playerBoughtCar;
    [SerializeField] public int carPrice;
    [SerializeField] public float speed;
    [SerializeField] public float fuelEfficiency;
    [SerializeField] public float gravity;
    [SerializeField] public int motorCount;

    private void Start()
    {
        carName = transform.name;
        speed = GetFloatPrefs("speed", speed);
        fuelEfficiency = GetFloatPrefs("fuelEfficiency", fuelEfficiency);
        gravity = GetFloatPrefs("gravity", gravity);
        motorCount = (int)GetFloatPrefs("motorCount", (float)motorCount);
        carPrice = (int)GetFloatPrefs("carPrice", carPrice);
        playerBoughtCar = Convert.ToBoolean(GetFloatPrefs("playerBoughtCar", Convert.ToSingle(playerBoughtCar)));
    }
    private float GetFloatPrefs(string parameter, float currentParameter)
    {
        if (!PlayerPrefs.HasKey(carName + parameter))
        {
            PlayerPrefs.SetFloat(carName + parameter, currentParameter);
        }
        return PlayerPrefs.GetFloat(carName + parameter);
    }
}
