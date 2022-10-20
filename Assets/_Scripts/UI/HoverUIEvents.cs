using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUIEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEvents.instance.HoverUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.instance.MouseLeaveUI();
    }
}
