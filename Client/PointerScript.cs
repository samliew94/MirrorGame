using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onHover;
    public UnityEvent onExit;
    public UnityEvent onClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }

    

    

    
}
