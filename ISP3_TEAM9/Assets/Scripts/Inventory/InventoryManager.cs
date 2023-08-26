using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Objects")]
    public List<InventorySlot> invSlots = new List<InventorySlot>();
    public GameObject invItemPrefab;
    public GameObject invSlotPrefab;

    public GameObject slotContainer;

    public Item lastItemAdded;

    int selectedSlot = -1;

    void Awake()
    {
        
    }

    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if (isNumber && number > 0 && number < 4)
            {
                ChangeSlot(number - 1);

                GameObject.FindWithTag("Player").GetComponent<UseItem>().UseSelectedItem(true);
            }
        }
    }

    void ChangeSlot(int newSlot)
    {
        selectedSlot = newSlot;
        Debug.Log("Selected slot " + selectedSlot);
    }

    public bool AddItem(Item item)
    {
        // Check if last slot is empty
        int invSlotCount = invSlots.Count();
        InventorySlot lastSlot = invSlots[invSlotCount - 2];

        if (lastSlot.GetComponentInChildren<InventoryItem>() != null)
        {
            for (int i = 0; i < 5; i++)
            {
                AddSlot();
            }
        }

        for (int i = 0; i < invSlots.Count; i++)
        {
            InventorySlot slot = invSlots[i];
            InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemSlot != null && itemSlot.item.stackable == true && itemSlot.item == item && itemSlot.count < 5)
            {
                itemSlot.count++;
                itemSlot.UpdateCount();
                lastItemAdded = item;
                return true;
            }
        }

        for (int i = 0; i < invSlots.Count; i++)
        {
            InventorySlot slot = invSlots[i];

            if (item.itemType == Item.types.Artefact)
            {
                slot = invSlots[i + 3];
            }

            if (slot.GetComponentInChildren<InventoryItem>() == null)
            {
                SpawnNewItem(item, slot);
                lastItemAdded = item;
                return true;
            }
        }

        return false;
    }

    void AddSlot()
    {
        GameObject newSlotGO = Instantiate(invSlotPrefab, slotContainer.transform);

        InventorySlot newSlot = newSlotGO.GetComponentInChildren<InventorySlot>();

        invSlots.Add(newSlot);
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(invItemPrefab, slot.transform);

        InventoryItem invItem = newItemGO.GetComponent<InventoryItem>();

        invItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool consumable, bool isHealthMax, bool isSprintPotion, bool isDefensePotion, bool isAttackPotion, bool isAtkSpdPotion)
    {
        InventorySlot slot = invSlots[selectedSlot];
        InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemSlot != null)
        {
            Item item = itemSlot.item;

            if (consumable)
            {
                if (item.itemName.Contains("Small Health Potion") && isHealthMax)
                {
                    Debug.Log("Health is full.");
                    //sysText.DisplayText("A Scroll of Swift is already in effect.");
                }
                else if (item.itemName.Contains("Big Health Potion") && isHealthMax)
                {
                    Debug.Log("Health is full.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.itemName.Contains("Defense Potion") && isDefensePotion)
                {
                    Debug.Log("Defense Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.itemName.Contains("Attack Speed Potion") && isAtkSpdPotion)
                {
                    Debug.Log("Attack Speed Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.itemName.Contains("Attack Potion") && isAttackPotion)
                {
                    Debug.Log("Attack Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.itemName.Contains("Sprint Potion") && isSprintPotion)
                {
                    Debug.Log("Sprint Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else
                {
                    itemSlot.count--;

                    if (itemSlot.count <= 0)
                    {
                        Destroy(itemSlot.gameObject);
                    }
                    else
                    {
                        itemSlot.UpdateCount();
                    }
                }
            }

            return item;
        }

        return null;
    }
}
