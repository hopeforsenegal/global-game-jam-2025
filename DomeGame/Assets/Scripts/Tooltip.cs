using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
        Debug.Log("Mouse Ocer");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
        Debug.Log("Mouse Exit");
    }
}
