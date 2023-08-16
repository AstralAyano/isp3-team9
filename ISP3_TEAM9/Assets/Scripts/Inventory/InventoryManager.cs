using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static GameObject instance;

    [Header("Objects")]
    public InventorySlot[] invSlots;
    public GameObject invItemPrefab;

    //public SystemText sysText;

    int selectedSlot = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if (isNumber && number > 0 && number < 10)
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
                return true;
            }
        }

        for (int i = 0; i < invSlots.Length; i++)
        {
            InventorySlot slot = invSlots[i];

            if (slot.GetComponentInChildren<InventoryItem>() == null)
            {
                SpawnNewItem(item, slot);
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

    public Item GetSelectedItem(bool consumable, bool isHealthMax, bool isManaMax)
    {
        InventorySlot slot = invSlots[selectedSlot];
        InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemSlot != null)
        {
            Item item = itemSlot.item;

            if (consumable)
            {
                if (item.name.Contains("Health Potion") && isHealthMax)
                {
                    Debug.Log("Health is full.");
                    //sysText.DisplayText("A Scroll of Swift is already in effect.");
                }
                else if (item.name.Contains("Mana Potion") && isManaMax)
                {
                    Debug.Log("Mana is full.");
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
