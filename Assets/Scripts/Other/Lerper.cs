using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    [SerializeField] private GameObject objectToMove, endPoint;
    [SerializeField] private bool repeatable = false;
    [SerializeField] private float speed = 1.0f;
    private GameObject startPoint;
    float startTime, totalDistance;

    [SerializeField] private bool stop = false;
     public bool Stop => stop;
    public Lerper(GameObject objectToMove,  GameObject endPoint, float speed, bool repeatable)
    {
        this.objectToMove = objectToMove;
        this.endPoint = endPoint;
        this.repeatable = repeatable;
        this.speed = speed;
    }
    IEnumerator Start()
    {
        startPoint = new GameObject();
        startPoint.transform.position = objectToMove.transform.position;
        startPoint.transform.rotation = objectToMove.transform.rotation;

        startTime = Time.time;
        totalDistance = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);
        while (repeatable)
        {
            yield return RepeatLerp(startPoint, endPoint, 15f);
            yield return RepeatLerp(endPoint, startPoint, 15);
        }
    }
    void Update()
    {

        if (!repeatable&&!stop)
        {
            float currentDuration = (Time.time - startTime) * speed;
            float journeyFraction = currentDuration / totalDistance;
            objectToMove.transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, journeyFraction);
            objectToMove.transform.rotation = Quaternion.Lerp(startPoint.transform.rotation, endPoint.transform.rotation, journeyFraction);
            Debug.Log(objectToMove.transform.position);
        }
    }

    public IEnumerator RepeatLerp(GameObject a, GameObject b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            if (!stop)
            {
                i += Time.deltaTime * rate;
                objectToMove.transform.position = Vector3.Lerp(a.transform.position, b.transform.position, i);
                objectToMove.transform.rotation = Quaternion.Lerp(a.transform.rotation, b.transform.rotation, i);
            }
            yield return null;
        }
     
    }
}
