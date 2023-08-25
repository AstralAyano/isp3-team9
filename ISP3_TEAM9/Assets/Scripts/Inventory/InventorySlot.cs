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

            if (invItem.item.itemType.ToString() == "Artefact")
            {
                if (transform.gameObject.CompareTag("ArtefactSlot"))
                {
                    invItem.parentAfterDrag = transform;
                    transform.parent.transform.parent.Find("Equip").GetComponent<AudioSource>().Play();
                }
                else if (transform.gameObject.CompareTag("InvSlot"))
                {
                    invItem.parentAfterDrag = transform;
                }
            }
            else if (invItem.item.itemType.ToString() == "Consumable")
            {
                if (transform.gameObject.CompareTag("ConsumableSlot"))
                {
                    invItem.parentAfterDrag = transform;
                }
                else if (transform.gameObject.CompareTag("InvSlot"))
                {
                    invItem.parentAfterDrag = transform;
                }
            }
            else if (transform.gameObject.CompareTag("InvSlot"))
            {
                invItem.parentAfterDrag = transform;
            }
        }
    }
}
