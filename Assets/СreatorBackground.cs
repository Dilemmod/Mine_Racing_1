using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class СreatorBackground : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private GameObject FirstBackgroundChunk;
    [SerializeField] private int OffsetBackgroundChunk;
    //private Transform lastPositionBackgroundChunk;

    private List<GameObject> spawnedBackgroundChunks = new List<GameObject>();
    private int[] degrees = {0, 90, 180, 270};

   // CarController carController;
    void Start()
    {
        spawnedBackgroundChunks.Add(FirstBackgroundChunk);
        FirstBackgroundChunk = Instantiate(FirstBackgroundChunk);

        //lastPositionBackgroundChunk = FirstBackgroundChunk.transform;
    }
    void Update()
    {
        if (Player.position.x > spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1].transform.position.x)
        {
            SpawnChunk();
        }
    }
    private void SpawnChunk()
    {
        GameObject newBackgroundChunk = Instantiate(spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1]);
        newBackgroundChunk.transform.position = spawnedBackgroundChunks[spawnedBackgroundChunks.Count-1].transform.position;
        newBackgroundChunk.transform.position = new Vector3(
            newBackgroundChunk.transform.position.x+ OffsetBackgroundChunk, 
            newBackgroundChunk.transform.position.y,
            newBackgroundChunk.transform.position.z);
        newBackgroundChunk.transform.Rotate(0, degrees[Random.Range(0, degrees.Length)],0);
        spawnedBackgroundChunks.Add(newBackgroundChunk);

        if (spawnedBackgroundChunks.Count >= 3)
        {
            if (FirstBackgroundChunk != null)
                Destroy(FirstBackgroundChunk);
            else
                Destroy(spawnedBackgroundChunks[0].gameObject);
            spawnedBackgroundChunks.RemoveAt(0);
        }
    }
}
