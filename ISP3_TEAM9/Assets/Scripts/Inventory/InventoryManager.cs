using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Objects")]
    public InventorySlot[] invSlots;
    public GameObject invItemPrefab;

    public Item lastItemAdded;

    //public SystemText sysText;

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
        for (int i = 0; i < invSlots.Length; i++)
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

        for (int i = 0; i < invSlots.Length; i++)
        {
            InventorySlot slot = invSlots[i];

            if (slot.GetComponentInChildren<InventoryItem>() == null)
            {
                SpawnNewItem(item, slot);
                lastItemAdded = item;
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(invItemPrefab, slot.transform);

        InventoryItem invItem = newItemGO.GetComponent<InventoryItem>();

        invItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool consumable, bool isHealthMax, bool isSprintPotion, bool isDefensePotion, bool isAttackPotion,bool IsAtkSpdPotion)
    {
        InventorySlot slot = invSlots[selectedSlot];
        InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemSlot != null)
        {
            Item item = itemSlot.item;

            if (consumable)
            {
                if (item.name.Contains("Small Health Potion") && isHealthMax)
                {
                    Debug.Log("Health is full.");
                    //sysText.DisplayText("A Scroll of Swift is already in effect.");
                }
                else if (item.name.Contains("Big Health Potion") && isHealthMax)
                {
                    Debug.Log("Health is full.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.name.Contains("Defense Potion") && isDefensePotion)
                {
                    Debug.Log("Defense Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.name.Contains("Attack Speed Potion") && IsAtkSpdPotion)
                {
                    Debug.Log("Attack Speed Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.name.Contains("Attack Potion") && isAttackPotion)
                {
                    Debug.Log("Attack Potion Is Already Being Used.");
                    //sysText.DisplayText("You can't use this scroll here.");
                }
                else if (item.name.Contains("Sprint Potion") && isSprintPotion)
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
