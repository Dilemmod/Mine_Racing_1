using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Grass;
    [SerializeField] private GameObject Dirt;
    [SerializeField] private GameObject StartPositionGameObject;
    private Vector3 StartPosition;

    //// Size of World
    //SizeX;
    [SerializeField] private uint RoadLength;
    //SizeZ;
    [SerializeField] private uint RoadWidth;
    //SizeY;
    [SerializeField] private uint RoadHeight;

    void Start()
    {
        StartPosition = StartPositionGameObject.transform.position;
        //Setting the correct parameters
        RoadLength = RoadLength / 2;
        RoadWidth = RoadWidth / 2;
        RoadHeight = RoadHeight / 2;
        StartPosition = new Vector3(StartPosition.x, StartPosition.y + 0.25f, StartPosition.z + 0.25f);
        //Create map
        StartCoroutine(SimpleGenerator());
    }

    public static void CloneAndPlace(Vector3 newPosition,GameObject originalGameobject)
    {
        // Клон
        GameObject clone = (GameObject)Instantiate(originalGameobject,  newPosition, Quaternion.identity);
        // Позиция
        clone.transform.position = newPosition;
        clone.name = "Block@" + clone.transform.position;
    }

    IEnumerator SimpleGenerator()
    {
        // В этом потоке мы будем создавать 50 кубов за один фрейм
        uint numberOfInstances = 0;
        uint instancesPerFrame = 50;
        float roadDecline = 0 ;

        for (float x = 0.5f; x <= RoadLength; x+=0.5f)
        {
            for (float z = 0.5f; z <= RoadWidth; z += 0.5f)
            {
                // Получаем случайную высоту
                float height = Random.Range(RoadHeight-1, RoadHeight)- roadDecline;
                for (float y = 0f; y <= height; y += 0.5f)
                {
                    // Расчитываем позицию для каждого блока
                    //-z needed for visualization from the side of the player
                    Vector3 newPosition = new Vector3(x, y, -z);
                    newPosition = newPosition + StartPosition;
                    // Вызываем метод, передавая ему новую позицию и экземпляр куба
                    if (y > height-0.5f)
                        CloneAndPlace(newPosition, Grass);
                    else
                        CloneAndPlace(newPosition, Dirt);
                    // Инкрементируем numberOfInstances
                    numberOfInstances++;

                    // Если было достигнуто предельное количество экземпляров за фрейм
                    if (numberOfInstances == instancesPerFrame)
                    {
                        // Сбрасываем numberOfInstances
                        numberOfInstances = 0;
                        // И ждем следующего фрейма
                        yield return new WaitForEndOfFrame();
                    }
                }
                roadDecline += 0.5f;
            }
            roadDecline = 0;
        }
    }
}
