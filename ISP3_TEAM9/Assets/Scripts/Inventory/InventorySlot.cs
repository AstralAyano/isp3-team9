using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [HideInInspector] public Transform parentAfterDrag;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem invItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            invItem.parentAfterDrag = transform;
        }
    }
}
