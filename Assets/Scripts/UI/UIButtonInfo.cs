using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonInfo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDown { get; set; }
    public void OnPointerDown(PointerEventData eventData)
    {
        this.isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.isDown = false;
    }
}
