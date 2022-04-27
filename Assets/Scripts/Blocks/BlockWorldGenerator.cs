using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BlockWorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Grass;
    [SerializeField] private GameObject Dirt;
    //[SerializeField] private GameObject StartPositionObject;

    //// Size of World
    //SizeX;
    [SerializeField] private uint RoadLength;
    //SizeZ;
    [SerializeField] private uint RoadWidth;
    //SizeY;
    //[SerializeField] private uint RoadHeight;

    private Vector3 startPosition;
    private GameObject blockContainer;
    private EdgeCollider2D edgCollider2D;

    private void Awake()
    {
        //Setting the correct parameters
        RoadWidth = (RoadWidth == 0 ? RoadWidth = 4/2 : RoadWidth/2);
        RoadLength = (RoadLength == 0 ? RoadLength = 60 / 2 : RoadLength / 2);
        blockContainer = new GameObject();
        blockContainer.name = "BlockContainer";
        //shape = this.gameObject.GetComponent<SpriteShapeController>();
        edgCollider2D = GetComponent<EdgeCollider2D>();
        Grass =  (Grass==null? Resources.Load("Blocks/GrassBlock") as GameObject:Grass);
        Dirt = (Dirt == null ? Resources.Load("Blocks/DirtBlock") as GameObject : Dirt);
        //startPosition = new Vector3(gameObject.transform.position.x, shape.spline.GetPosition(0).y, 0);
        //RoadLength = Convert.ToUInt32(shape.spline.GetPosition(shape.spline.GetPointCount() - 1).x*2);
    }
    void Start()
    {
        //Create Block container
        blockContainer.transform.parent = this.gameObject.transform;
        Instantiate(blockContainer);
        //Start pos
        startPosition = new Vector3(blockContainer.transform.position.x, edgCollider2D.points[0].y, 0);
        //if (StartPositionObject != null)
            //startPosition = StartPositionObject.transform.position;
        startPosition = new Vector3(startPosition.x-0.25f, startPosition.y + 0.25f, startPosition.z + 0.25f);
        //Create map
        StartCoroutine(SimpleGenerator());
    }

    public static void CloneAndPlace(Vector3 newPosition, GameObject originalGameobject, GameObject parentGameObject)
    {
        // Clone
        GameObject clone = (GameObject)Instantiate(originalGameobject,  newPosition, Quaternion.identity);
        // Position
        clone.transform.position = newPosition;
        clone.name = "Block@" + clone.transform.position;
        // Perent folder
        clone.transform.parent = parentGameObject.transform;
    }

    IEnumerator SimpleGenerator()
    {
        //50 block per frame
        uint numberOfInstances = 0;
        uint instancesPerFrame = 50;
        float roadDecline = 0 ;

        int shapeIndex = 1;
        Vector3 roadPoint = edgCollider2D.points[shapeIndex];

        for (float x = 0.5f; x <= RoadLength; x+=0.5f)
        {
            for (float z = 0.5f; z <= RoadWidth; z += 0.5f)
            {
                // Random heigth
                float height = UnityEngine.Random.Range(roadPoint.y-0.7f+3, roadPoint.y+3) - roadDecline;
                for (float y = 0f; y <= height; y += 0.5f)
                {
                    //-z needed for visualization from the side of the player
                    Vector3 newPosition = new Vector3(x, y, -z);
                    newPosition = newPosition + startPosition;

                    // New position for block
                    //Last block with grass
                    if (y > height - 0.5f)
                    {
                        CloneAndPlace(newPosition, Grass, blockContainer);
                        //Debug.Log("Pos = "+newPosition);
                    }
                    else
                        CloneAndPlace(newPosition, Dirt, blockContainer);
                    numberOfInstances++;

                    //If the limit of instances per frame has been reached
                    if (numberOfInstances == instancesPerFrame)
                    {
                        numberOfInstances = 0;
                        //Waiting for the next frame
                        yield return new WaitForEndOfFrame();
                    }
                }
                roadDecline += 0.5f;
            }
            //Find current roadPoint 
            while (roadPoint.x < x)
            {
                shapeIndex++;
                roadPoint = edgCollider2D.points[shapeIndex];
            }
            //Debug.Log("roadPoint = " + roadPoint);
            roadDecline = 0;
        }
    }
}
