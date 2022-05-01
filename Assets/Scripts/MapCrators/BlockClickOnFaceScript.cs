using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClickOnFaceScript : MonoBehaviour
{
    //private FirstPersonController firstPersonController;
    private GameObject perentObject;
    private GameObject grassBlock;
    private GameObject dirtBlock;
    private GameObject snowBlock;
    private GameObject gravelBlock;
    private GameObject sandBlock;
    private GameObject sandStoneBlock1;
    private GameObject sandStoneBlock2;
    private GameObject sandStoneBlock3;

    private GameObject usedBlock;
    private Vector3[] AllRotation;

    // Хранит смещение, требуемое для расчета позиции нового объекта
    private Vector3 delta;
    Vector3 newPosition;
    private void Awake()
    {
        AllRotation = new[] { 
            new Vector3(90f, 0f, 0f),
            new Vector3(90f, 90f, 0f), 
            new Vector3(90f, -90f, 0f),
            new Vector3(90f, -180f, 0f),
            new Vector3(90f, 180f, 0f) };
        perentObject = GameObject.FindGameObjectWithTag("BlocksParent");
        gravelBlock= Resources.Load("Blocks/GravelBlock") as GameObject;
        grassBlock = Resources.Load("Blocks/GrassBlock") as GameObject;
        dirtBlock = Resources.Load("Blocks/DirtBlock") as GameObject;
        snowBlock = Resources.Load("Blocks/SnowBlock") as GameObject;
        sandBlock = Resources.Load("Blocks/SandBlock") as GameObject;
        sandStoneBlock1 = Resources.Load("Blocks/SandStoneBlock (1)") as GameObject;
        sandStoneBlock2 = Resources.Load("Blocks/SandStoneBlock (2)") as GameObject;
        sandStoneBlock3 = Resources.Load("Blocks/SandStoneBlock (3)") as GameObject;
        usedBlock = this.transform.parent.gameObject;
        delta.x = this.transform.localPosition.x;
        delta.y = this.transform.localPosition.y;
        delta.z = this.transform.localPosition.z;
    }
    private void Update()
    {
        var input = Input.inputString;
        switch (input)
        {
            case "1":
                usedBlock = grassBlock;
                break;
            case "2":
                usedBlock = dirtBlock;
                break;
            case "3":
                usedBlock = snowBlock;
                break;
            case "4":
                usedBlock = sandBlock;
                break;
            case "5":
                usedBlock = sandStoneBlock1;
                break;
            case "6":
                usedBlock = sandStoneBlock2;
                break;
            case "7":
                usedBlock = sandStoneBlock3;
                break;
        }
    }
    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Destroy(this.transform.parent.gameObject);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            newPosition = this.transform.parent.transform.position + delta;
            // Клон
            GameObject clone = Instantiate(usedBlock, newPosition, Quaternion.identity);
            // Позиция
            clone.transform.position = newPosition;
            // Поворот блока, для лучшего отображения
            clone.transform.GetChild(0).eulerAngles = AllRotation[Random.Range(0, 4)];
            // Переименовывем
            clone.name = "Block@" + clone.transform.position;
            //Перенос в папку родителя
            if (perentObject != null)
                clone.transform.parent = perentObject.transform;
        }
    }
}
