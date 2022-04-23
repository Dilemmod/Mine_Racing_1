using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreator : MonoBehaviour
{
    public GameObject gameObjectBetween;
    private EdgeCollider2D edgCollider2D;
    Vector3 pointA = new Vector3();
    Vector3 pointB = new Vector3();
    private void CreatingShapeBetweenPoints(Vector3 pA, Vector3 pB)
    {
        if (gameObjectBetween == null)
            return;
        GameObject cloneBetween = Instantiate(gameObjectBetween);
        Vector3 between = pB - pA;
        float distance = between.magnitude;
        cloneBetween.transform.localPosition = pA + (between / 2.0f);
        cloneBetween.transform.localScale = new Vector3(distance+0.1f, cloneBetween.transform.localScale.y, cloneBetween.transform.localScale.z);

        //Переворот
        Quaternion tempRotation = cloneBetween.transform.localRotation;
        cloneBetween.transform.LookAt(pA);
        float tempX = cloneBetween.transform.eulerAngles.x;
        cloneBetween.transform.localRotation = tempRotation;
        if (pA.x < pB.x)
            cloneBetween.transform.Rotate(0, 0, tempX);
        else
            cloneBetween.transform.Rotate(0, 0, -tempX);
        //Перенос в папку родителя
        cloneBetween.transform.parent = this.gameObject.transform;
    }
    private void Awake()
    {
        edgCollider2D = GetComponent<EdgeCollider2D>();
    }
    void Start()
    {
        for (int i = 0; i < edgCollider2D.pointCount-1; i++)
        {
            
            pointA = new Vector2(edgCollider2D.points[i].x + this.transform.position.x, edgCollider2D.points[i].y + this.transform.position.y);
            pointB = new Vector2(edgCollider2D.points[i+1].x + this.transform.position.x, edgCollider2D.points[i+1].y + this.transform.position.y);
            //1.75 - 0.5 = 1.25 для точного размешения блоков
            //pointA.z = -0.5f;
            pointA.z = 0.75f;
            pointB.z = 0.75f;
            CreatingShapeBetweenPoints(pointA, pointB);
        }
    }
}