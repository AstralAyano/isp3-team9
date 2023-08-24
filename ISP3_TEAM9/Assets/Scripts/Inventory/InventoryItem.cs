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

    [SerializeField] private UIBookController uiBook;

    void Start()
    {
        uiBook = GameObject.FindWithTag("UI").GetComponentInChildren<UIBookController>(true);
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
                if (item.itemType.ToString() == "Artefact" &&
                    item.itemName == soItems[i].itemName)
                {
                    uiBook.ArtefactIncreaseStats(soItems[i].classResonance, 0, soItems[i].health, soItems[i].healthMulti);
                    uiBook.ArtefactIncreaseStats(soItems[i].classResonance, 1, soItems[i].defense, soItems[i].defenseMulti);
                    uiBook.ArtefactIncreaseStats(soItems[i].classResonance, 2, soItems[i].attack, soItems[i].attackMulti);
                    uiBook.ArtefactIncreaseStats(soItems[i].classResonance, 3, soItems[i].attackSpeed, soItems[i].attackSpeedMulti);
                    uiBook.ArtefactIncreaseStats(soItems[i].classResonance, 4, soItems[i].moveSpeed, soItems[i].moveSpeedMulti);
                    uiBook.ArtefactIncreaseStats(soItems[i].classResonance, 5, soItems[i].projectileSpeed, soItems[i].projectileSpeedMulti);
                }
            }
        }
        else if (parentAfterDrag.gameObject.CompareTag("InvSlot") &&
                !parentBeforeDrag.gameObject.CompareTag("InvSlot"))
        {
            for (int i = 0; i < soItems.Length; i++)
            {
                if (item.itemType.ToString() == "Artefact" &&
                    item.itemName == soItems[i].itemName)
                {
                    uiBook.ArtefactDecreaseStats(soItems[i].classResonance, 0, soItems[i].health, soItems[i].healthMulti);
                    uiBook.ArtefactDecreaseStats(soItems[i].classResonance, 1, soItems[i].defense, soItems[i].defenseMulti);
                    uiBook.ArtefactDecreaseStats(soItems[i].classResonance, 2, soItems[i].attack, soItems[i].attackMulti);
                    uiBook.ArtefactDecreaseStats(soItems[i].classResonance, 3, soItems[i].attackSpeed, soItems[i].attackSpeedMulti);
                    uiBook.ArtefactDecreaseStats(soItems[i].classResonance, 4, soItems[i].moveSpeed, soItems[i].moveSpeedMulti);
                    uiBook.ArtefactDecreaseStats(soItems[i].classResonance, 5, soItems[i].projectileSpeed, soItems[i].projectileSpeedMulti);
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
