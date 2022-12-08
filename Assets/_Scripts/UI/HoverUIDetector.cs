using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUIDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIEvents.instance.HoverUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEvents.instance.MouseLeaveUI();
    }
}
