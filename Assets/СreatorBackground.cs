using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class СreatorBackground : MonoBehaviour
{
    //[SerializeField] private Transform Player;
    private CarController carController;
    [SerializeField] private GameObject FirstBackgroundChunk;
    [SerializeField] private int OffsetBackgroundChunk;
    [SerializeField] private int extraOffset;
    //private Transform lastPositionBackgroundChunk;

    private List<GameObject> spawnedBackgroundChunks = new List<GameObject>();
    private int[] degrees = { 0, 90, 180, 270 };

    private void Awake()
    {
        
    }
    void Start()
    {
        carController = CarController.Instance;
        //First GameObject
        spawnedBackgroundChunks.Add(FirstBackgroundChunk);
        FirstBackgroundChunk = Instantiate(FirstBackgroundChunk);
        FirstBackgroundChunk.transform.parent = this.transform;
    }
    void Update()
    {
        if (carController.GetPlayerPosition().x + extraOffset > spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1].transform.position.x)
        {
            SpawnChunk();
        }
    }
    private void SpawnChunk()
    {
        GameObject newBackgroundChunk = Instantiate(spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1]);
        newBackgroundChunk.transform.position = spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1].transform.position;
        newBackgroundChunk.transform.position = new Vector3(
            newBackgroundChunk.transform.position.x + OffsetBackgroundChunk,
            newBackgroundChunk.transform.position.y,
            newBackgroundChunk.transform.position.z);
        newBackgroundChunk.transform.Rotate(0, degrees[Random.Range(0, degrees.Length)], 0);
        newBackgroundChunk.transform.parent = this.transform;
        spawnedBackgroundChunks.Add(newBackgroundChunk);

        if (spawnedBackgroundChunks.Count >= 4)
        {
            if (FirstBackgroundChunk != null)
                Destroy(FirstBackgroundChunk);
            else
                Destroy(spawnedBackgroundChunks[0].gameObject);
            spawnedBackgroundChunks.RemoveAt(0);
        }
    }
}
