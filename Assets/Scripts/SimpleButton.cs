using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent action;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        action?.Invoke();
    }
}
