using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image image;
    public TMP_Text countText;
    public GameObject invUI;
    public InventoryUI ui;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    void Start()
    {
        invUI = GameObject.Find("InventoryUI");
        ui = invUI.GetComponent<InventoryUI>();
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;

        UpdateCount();
    }

    public void UpdateCount()
    {
        countText.text = count.ToString();

        if (count > 1)
        {
            countText.gameObject.SetActive(true);
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //ui.nameText.text = item.itemName;
        //ui.descText.text = item.itemDesc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ui.nameText.text = "";
        //ui.descText.text = "";
    }
}
