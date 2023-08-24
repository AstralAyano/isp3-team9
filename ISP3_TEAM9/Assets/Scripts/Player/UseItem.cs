using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [Header("Objects")]
    public InventorySlot[] invSlots;
    public InventoryManager invManager;

    int selectedSlot = -1;

    [HideInInspector] public PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if (isNumber && number > 0 && number < 4)
            {
                UseSelectedItem(true);
            }
        }
    }


    public Item UseSelectedItem(bool consumable)
    {
        Item usingItem = invManager.GetSelectedItem(true, controller.IsHealthMax, controller.IsSpeedPotionActive ,controller.IsAtkPotionActive, controller.IsAtkSpdPotionActive, controller.IsDefensePotionActive);
        InventorySlot slot = invSlots[selectedSlot];
        InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemSlot != null)
        {
            Item item = itemSlot.item;

            if (consumable)
            {
                if (item.name.Contains("Small Health Potion") && controller.IsHealthMax)
                {
                    Debug.Log("Health is full.");
                    //sysText.DisplayText("A Scroll of Swift is already in effect.");
                }
                else if (item.name.Contains("Big Health Potion") && controller.IsHealthMax)
                {
                    Debug.Log("Health is full.");
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
