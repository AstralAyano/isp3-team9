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

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentBeforeDrag;
    [HideInInspector] public Transform parentAfterDrag;

    [SerializeField] private Item[] soItems;
    [SerializeField] private ScriptablePlayerStats playerStats;

    void Start()
    {
        
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
        parentBeforeDrag = transform.parent;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);

        if (parentAfterDrag.gameObject.CompareTag("ArtefactSlot") &&
            !parentBeforeDrag.gameObject.CompareTag("ArtefactSlot"))
        {
            for (int i = 0; i < soItems.Length; i++)
            {
                if (soItems[i].itemType.ToString() == "Artefact" &&
                    item.name == soItems[i].itemName)
                {
                    playerStats.chosenStatPoints.health += soItems[i].health;
                    playerStats.chosenStatPoints.maxHealth += soItems[i].maxHealth;
                    playerStats.chosenStatPoints.attack += soItems[i].attack;
                    playerStats.chosenStatPoints.defense += soItems[i].defense;
                    playerStats.chosenStatPoints.attackSpeed += soItems[i].attackSpeed;
                    playerStats.chosenStatPoints.moveSpeed += soItems[i].moveSpeed;
                    playerStats.chosenStatPoints.projectileSpeed += soItems[i].projectileSpeed;
                }
            }
        }
        else if (parentAfterDrag.gameObject.CompareTag("InvSlot") &&
                !parentBeforeDrag.gameObject.CompareTag("InvSlot"))
        {
            for (int i = 0; i < soItems.Length; i++)
            {
                if (soItems[i].itemType.ToString() == "Artefact" &&
                    item.name == soItems[i].itemName)
                {
                    playerStats.chosenStatPoints.health -= soItems[i].health;
                    playerStats.chosenStatPoints.maxHealth -= soItems[i].maxHealth;
                    playerStats.chosenStatPoints.attack -= soItems[i].attack;
                    playerStats.chosenStatPoints.defense -= soItems[i].defense;
                    playerStats.chosenStatPoints.attackSpeed -= soItems[i].attackSpeed;
                    playerStats.chosenStatPoints.moveSpeed -= soItems[i].moveSpeed;
                    playerStats.chosenStatPoints.projectileSpeed -= soItems[i].projectileSpeed;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.instance.SetAndShowToolTip(item.itemName, item.itemType.ToString(), item.itemDesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.instance.HideToolTip();
    }
}
