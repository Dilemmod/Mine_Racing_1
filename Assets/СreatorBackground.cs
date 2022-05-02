using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class СreatorBackground : MonoBehaviour
{
    //[SerializeField] private Transform Player;
    private CarController carController;
    [Header("Chunks")]
    [SerializeField] private GameObject[] BackgroundChunks;
    //[SerializeField] private GameObject FirstBackgroundChunk;
    //[SerializeField] private GameObject SecondBackgroundChunk;
    // [SerializeField] private GameObject ThirdBackgroundChunk;
    //[SerializeField] private GameObject FourthBackgroundChunk;
    private GameObject BackgroundChunk;
    [Header("Offset")]
    [SerializeField] private int OffsetBackgroundChunk =60;
    [SerializeField] private int ExtraOffset = 30;
    //private Transform lastPositionBackgroundChunk;

    private List<GameObject> spawnedBackgroundChunks = new List<GameObject>();
   // private List<GameObject> backgroundChunks = new List<GameObject>();
    private int[] degrees = { 0, 90, 180, 270 };
    /*
    private void Awake()
    {
        if (FirstBackgroundChunk != null)
            backgroundChunks.Add(FirstBackgroundChunk);
        if(SecondBackgroundChunk != null)
            backgroundChunks.Add(SecondBackgroundChunk);
        if (ThirdBackgroundChunk != null)
            backgroundChunks.Add(ThirdBackgroundChunk);
        if (FourthBackgroundChunk != null)
            backgroundChunks.Add(FourthBackgroundChunk);
    }*/
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
        newBackgroundChunk.transform.position = spawnedBackgroundChunks[spawnedBackgroundChunks.Count - 1].transform.position;
        newBackgroundChunk.transform.position = new Vector3(    
            newBackgroundChunk.transform.position.x + OffsetBackgroundChunk,
            newBackgroundChunk.transform.position.y,
            newBackgroundChunk.transform.position.z);
        newBackgroundChunk.transform.Rotate(0, newBackgroundChunk.GetComponent<HowRotateObject>().GetRandomRotate(), 0);
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
