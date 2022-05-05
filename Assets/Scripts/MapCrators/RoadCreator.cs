using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreator : MonoBehaviour
{
    private GameObject roadPlaneGameObject;
    private EdgeCollider2D edgCollider2D;
    private GameObject tempRoadPlaneGameObject;
    Vector3 pointA = new Vector3();
    Vector3 pointB = new Vector3();
    private void Awake()
    {
        roadPlaneGameObject = Resources.Load<GameObject>("Prefabs/RoadObjects/RoadPlane");
        tempRoadPlaneGameObject = new GameObject();
        edgCollider2D = GetComponent<EdgeCollider2D>();
    }
    void Start()
    {
        Destroy(tempRoadPlaneGameObject);
        //Parent object
        tempRoadPlaneGameObject = Instantiate(tempRoadPlaneGameObject);
        tempRoadPlaneGameObject.name = "RoadObjectContainer";
        tempRoadPlaneGameObject.transform.parent = this.gameObject.transform;
        //Create road
        for (int i = 1; i < edgCollider2D.pointCount - 5; i++)
        {
            pointA = new Vector2(edgCollider2D.points[i].x + this.transform.position.x, edgCollider2D.points[i].y + this.transform.position.y);
            pointB = new Vector2(edgCollider2D.points[i + 1].x + this.transform.position.x, edgCollider2D.points[i + 1].y + this.transform.position.y);
            pointA.z = 1f;
            pointB.z = 1f;
            CreatingShapeBetweenPoints(pointA, pointB, tempRoadPlaneGameObject);
        }
    }
    private void CreatingShapeBetweenPoints(Vector3 pA, Vector3 pB, GameObject parentobject)
    {
        if (roadPlaneGameObject == null)
        {
            Debug.LogError("RoadPlaneGameObject is absent");
            return;
        }
        GameObject cloneBetween = Instantiate(roadPlaneGameObject);
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
}