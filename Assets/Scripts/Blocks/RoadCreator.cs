using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreator : MonoBehaviour
{
    public GameObject gameObjectBetween;
    private EdgeCollider2D edgCollider2D;
    private GameObject roadObjectContainer;
    Vector3 pointA = new Vector3();
    Vector3 pointB = new Vector3();
    private void CreatingShapeBetweenPoints(Vector3 pA, Vector3 pB,GameObject parentobject)
    {
        if (gameObjectBetween == null)
            return;
        GameObject cloneBetween = Instantiate(gameObjectBetween);
        Vector3 between = pB - pA;
        float distance = between.magnitude;
        cloneBetween.transform.localPosition = pA + (between / 2.0f);
        cloneBetween.transform.localScale = new Vector3(distance, cloneBetween.transform.localScale.y, cloneBetween.transform.localScale.z);

        //Overturn
        Quaternion tempRotation = cloneBetween.transform.localRotation;
        cloneBetween.transform.LookAt(pA);
        float tempX = cloneBetween.transform.eulerAngles.x;
        cloneBetween.transform.localRotation = tempRotation;
        if (pA.x < pB.x)
            cloneBetween.transform.Rotate(0, 0, tempX);
        else
            cloneBetween.transform.Rotate(0, 0, -tempX);
        //Move to parent folder
        cloneBetween.transform.parent = parentobject.transform;
    }
    private void Awake()
    {
        roadObjectContainer = new GameObject();
        edgCollider2D = GetComponent<EdgeCollider2D>();
    }
    void Start()
    {
        //Parent object
        Instantiate(roadObjectContainer);
        roadObjectContainer.name = "RoadObjectContainer";
        roadObjectContainer.transform.parent = this.gameObject.transform;
        //Create road
        //for (int i = 0; i < edgCollider2D.pointCount-1; i++)
        for (int i = 1; i < edgCollider2D.pointCount-5; i++)
        {
            pointA = new Vector2(edgCollider2D.points[i].x + this.transform.position.x, edgCollider2D.points[i].y + this.transform.position.y);
            pointB = new Vector2(edgCollider2D.points[i+1].x + this.transform.position.x, edgCollider2D.points[i+1].y + this.transform.position.y);
            //1.75 - 0.5 = 1.25 for corect position
            //pointA.z = -0.5f;
            pointA.z = 0.75f;
            pointB.z = 0.75f;
            CreatingShapeBetweenPoints(pointA, pointB, roadObjectContainer);
        }
    }
}