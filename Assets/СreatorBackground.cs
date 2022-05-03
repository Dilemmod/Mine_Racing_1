using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class СreatorBackground : MonoBehaviour
{
    private CarController carController;
    [Header("Chunks")]
    [SerializeField] private GameObject[] BackgroundChunks;
    private GameObject BackgroundChunk;
    [Header("Offset")]
    [SerializeField] private float OffsetBackgroundChunk =60;
    [SerializeField] private float ExtraOffset = 30;

    private List<GameObject> spawnedBackgroundChunks = new List<GameObject>();
    void Start()
    {
        carController = CarController.Instance;
        //First GameObject
        BackgroundChunk = BackgroundChunks[Random.Range(0, BackgroundChunks.Length)];
        spawnedBackgroundChunks.Add(BackgroundChunk);
        BackgroundChunk = Instantiate(BackgroundChunk);
        BackgroundChunk.transform.parent = this.transform;
    }
    void Update()
    {
        if (carController.GetPlayerPosition().x + ExtraOffset > spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1].transform.position.x)
        {
            SpawnChunk();
        }
    }
    private void SpawnChunk()
    {
        GameObject newBackgroundChunk = Instantiate(BackgroundChunks[Random.Range(0, BackgroundChunks.Length)]);
        newBackgroundChunk.transform.position = new Vector3(
            spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1].transform.position.x + OffsetBackgroundChunk,
            newBackgroundChunk.transform.position.y,
            newBackgroundChunk.transform.position.z);
        float degrees = 0;
        if (newBackgroundChunk.GetComponent<HowRotateObject>()!=null)
            degrees = newBackgroundChunk.GetComponent<HowRotateObject>().GetRandomRotate();
        newBackgroundChunk.transform.Rotate(0, degrees, 0);
        newBackgroundChunk.transform.parent = this.transform;
        spawnedBackgroundChunks.Add(newBackgroundChunk);

        if (spawnedBackgroundChunks.Count >= 4)
        {
            if (BackgroundChunk != null)
                Destroy(BackgroundChunk);
            else
                Destroy(spawnedBackgroundChunks[0].gameObject);
            spawnedBackgroundChunks.RemoveAt(0);
        }
    }
}
