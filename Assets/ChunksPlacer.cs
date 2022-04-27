using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ChunksPlacer : MonoBehaviour
{
    [SerializeField] private Transform Player;
    //[SerializeField] private GameObject ChanksContainerPrefab;
    [SerializeField] private GameObject FirstChunk;

    Object[] obj;
    GameObject[] chunks;
    Vector3[] chunksBeginPositions;
    Vector3[] chunksEndPositions;
    private List<GameObject> spawnChunks = new List<GameObject>();

    CarController carController;
    void Start()
    {
        spawnChunks.Add(FirstChunk);
        FirstChunk = Instantiate(FirstChunk);
        
        carController = Player.GetComponent<CarController>();
        //chunks[0] = Resources.Load<GameObject>("Prefabs/RoadObjects/RoadChunks") as GameObject;
        chunks = Resources.LoadAll<GameObject>("Prefabs/RoadObjects/RoadChunks");
        //Debug.Log(chunks.Length);
        //Debug.Log("Chunk = " + chunks[11].name + "\nType = " + chunks[11].GetType() + "\n\n GetBeginPosition = " + GetBeginPosition(chunks[11]));
        /*
        chunksCount = ChanksContainerPrefab.transform.childCount;
        //Get all chunks in chunks array
        chunks = new GameObject[chunksCount];
        if (chunks.Length > 0)
            for (int i = 0; i < chunksCount; i++)
                chunks[i] = ChanksContainerPrefab.transform.GetChild(i).gameObject;
        */
        //Gat begin postion of chunk
        chunksBeginPositions = new Vector3[chunks.Length];
        chunksEndPositions = new Vector3[chunks.Length];
        for (int i = 0; i < chunks.Length; i++)
        {
            chunksBeginPositions[i] = GetBeginPosition(chunks[i]);
            chunksEndPositions[i] = GetEndPosition(chunks[i]);
        }
    }
    void Update()
    {
        if(Player.position.x > spawnChunks[spawnChunks.Count - 1].transform.position.x)
        {
            SpawnChunk();
        }
    }
    private Vector3 GetBeginPosition(GameObject chunk)
    {
        SpriteShapeController shape = chunk.gameObject.GetComponent<SpriteShapeController>();
        if (shape == null)
            return new Vector3(0, 0, 0);
        return shape.spline.GetPosition(1);
    }
    private GameObject GetRandomChunk(Vector3 lastChunkEndPosition)
    {
        GameObject neededChunk = new GameObject();
        //List<GameObject> neededChunks = new List<GameObject>();
        List<GameObject> neededChunksEasy = new List<GameObject>();
        List<GameObject> neededChunksMedium = new List<GameObject>();
        List<GameObject> neededChunksHard = new List<GameObject>();
        //Find all needed Chunks
        for (int i = 0; i < chunks.Length; i++)
        {
            if(chunksBeginPositions[i].y == lastChunkEndPosition.y)
            {
                if (chunks[i].gameObject.tag == "ChunkHard")
                    neededChunksHard.Add(chunks[i]);
                else if (chunks[i].gameObject.tag == "ChunkMedium")
                    neededChunksMedium.Add(chunks[i]);
                else if (chunks[i].gameObject.tag == "ChunkEasy")
                    neededChunksEasy.Add(chunks[i]);
            }
        }
        //Find Difficult of Chuncs
        int percent = Random.Range(0, 100);
        int playerDifficulty = 
            (carController.GetPlayerTravelDistance() > 200 ? 3 : 
            (carController.GetPlayerTravelDistance() > 100 ? 2 : 1));
        try
        {
            if (percent < 10) //10%
            {
                switch (playerDifficulty)
                {
                    case 3:
                        neededChunk = neededChunksEasy[Random.Range(0, neededChunksEasy.Count)];
                        break;
                    case 2:
                        neededChunk = neededChunksHard[Random.Range(0, neededChunksHard.Count)];
                        break;
                    case 1:
                        neededChunk = neededChunksHard[Random.Range(0, neededChunksHard.Count)];
                        break;
                }
            }
            else if (percent < 70) //60%
            {
                switch (playerDifficulty)
                {
                    case 3:
                        neededChunk = neededChunksHard[Random.Range(0, neededChunksHard.Count)];
                        break;
                    case 2:
                        neededChunk = neededChunksMedium[Random.Range(0, neededChunksMedium.Count)];
                        break;
                    case 1:
                        neededChunk = neededChunksEasy[Random.Range(0, neededChunksEasy.Count)];
                        break;
                }
            }
            else if (percent < 100) //30%
            {
                switch (playerDifficulty)
                {
                    case 3:
                        neededChunk = neededChunksMedium[Random.Range(0, neededChunksMedium.Count)];
                        break;
                    case 2:
                        neededChunk = neededChunksEasy[Random.Range(0, neededChunksEasy.Count)];
                        break;
                    case 1:
                        neededChunk = neededChunksMedium[Random.Range(0, neededChunksMedium.Count)];
                        break;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Can't find the needed Chunk\n{0} Exception caught." + e);
        }
        return neededChunk;
    }
    private Vector3 GetEndPosition(GameObject chunk)
    {
        SpriteShapeController shape = chunk.gameObject.GetComponent<SpriteShapeController>();
        if (shape == null)
            return new Vector3(0,0,0);
        return shape.spline.GetPosition(shape.spline.GetPointCount() - 2);
    }
    private void SpawnChunk()
    {
        Vector3 lastChunkEndPosition = GetEndPosition(spawnChunks[spawnChunks.Count - 1]);
        GameObject newChunk = Instantiate(GetRandomChunk(lastChunkEndPosition));
        newChunk.transform.position = new Vector3(spawnChunks[spawnChunks.Count - 1].transform.position.x + lastChunkEndPosition.x, newChunk.transform.position.y);
        spawnChunks.Add(newChunk);
        //Debug.Log("Distane = "+carController.GetPlayerTravelDistance()+"\nNewChunk.name   =   "+newChunk.name);

        if (spawnChunks.Count >= 4)
        {
            if(FirstChunk!=null)
                Destroy(FirstChunk);
            else 
                Destroy(spawnChunks[0].gameObject);
            spawnChunks.RemoveAt(0);
        }
    }
}
