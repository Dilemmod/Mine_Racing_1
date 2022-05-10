using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMainMenu : MonoBehaviour
{
    [Header("CameraRotation")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationYEnd = 90f;
    [SerializeField] private float rotationYBegin = 10f;
    private bool firstTurn = true;
    private float rotationY;
    private bool rotateOn = true;
    public bool RotateOn => rotateOn;

    [Header("CameraMoving")]
    [SerializeField] private GameObject mainMenuPosition;
    [SerializeField] private GameObject playerMenuPosition;
    [SerializeField] private GameObject levelMenuPosition;
    [SerializeField] private GameObject tuningMenuPosition;
    [SerializeField] private int speed = 1;
    private GameObject target;
    bool stop = true;
    public enum MenuPosition
    {
        mainTarget,
        playerTarget,
        levelTarget,
        tuningTarget
    }
    [SerializeField] public MenuPosition moveTo = MenuPosition.mainTarget;
    private MenuPosition moveTarget;
    private MenuPosition tempMoveTo;
    #region Singleton
    public static CameraControllerMainMenu Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    #region CameraRotation
    void CameraRotate(Vector3 direction)
    {
        rotationY = transform.localRotation.eulerAngles.y;
        //Проверка на конец поворота
        if ((rotationY > rotationYEnd && firstTurn) ||
            (rotationY < rotationYBegin && !firstTurn))
        {
            if (direction == Vector3.up)
                firstTurn = false;
            else if (direction == Vector3.down)
                firstTurn = true;
        }
        Camera.main.transform.Rotate(direction * rotationSpeed * Time.deltaTime, Space.World);
    }

    #endregion
    #region CameraMoving
    GameObject ConvertToMenuPosition(MenuPosition index)
    {
        switch (index)
        {
            case (MenuPosition)0:
                return mainMenuPosition;
            case (MenuPosition)1:
                return playerMenuPosition;
            case (MenuPosition)2:
                return levelMenuPosition;
            case (MenuPosition)3:
                return tuningMenuPosition;
        }
        return mainMenuPosition;
    }
    bool TimeToStop(MenuPosition index)
    {
        return Vector3.Distance(Camera.main.transform.position, ConvertToMenuPosition(index).transform.position) < 0.01f;
    }
    void MoveCameraTo(MenuPosition index)
    {
        target = ConvertToMenuPosition(index);
        Camera.main.transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, speed * Time.deltaTime);
    }
    #endregion
    public void CameraPosition(MenuPosition index)
    {
        tempMoveTo = moveTo;
        moveTo = (MenuPosition)index;
    }
    void FixedUpdate()
    {
        //CameraRotate
        rotateOn = (moveTo == (MenuPosition)0 ? true : false);
        if (rotateOn && stop)
        {
            if (firstTurn)
            {
                CameraRotate(Vector3.up);
            }
            else
            {
                CameraRotate(Vector3.down);
            }
        }
        //CameraMove
        if (tempMoveTo != moveTo) 
        {
            moveTarget = moveTo;
        }
        stop = (TimeToStop(moveTarget) ? true : false);
        if (!stop)
            MoveCameraTo(moveTo);
    }
}
