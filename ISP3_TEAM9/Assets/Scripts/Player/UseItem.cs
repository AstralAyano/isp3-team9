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
        Item usingItem = invManager.GetSelectedItem(true, controller.IsHealthMax, controller.IsSpeedPotionActive, controller.IsDefensePotionActive, controller.IsAtkPotionActive, controller.IsAtkSpdPotionActive);
        InventorySlot slot = invSlots[selectedSlot];
        InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemSlot != null)
        {
            Item item = itemSlot.item;

            if (consumable)
            {
                if (item.name.Contains("Small Health Potion") && !controller.IsHealthMax)
                {
                    controller.GainHP(item.HealPercent);

                }
                else if (item.name.Contains("Big Health Potion") && !controller.IsHealthMax)
                {
                    controller.GainHP(item.HealPercent);
                }
                else if (item.name.Contains("Defense Potion") && !controller.IsDefensePotionActive)
                {
                    controller.GainDefenseBoost(item.DefensePercent);
                }
                else if (item.name.Contains("Attack Speed Potion") && !controller.IsAtkSpdPotionActive)
                {
                    controller.GainAtkSpdBoost(item.AttackSpdPercent);
                }
                else if (item.name.Contains("Attack Potion") && !controller.IsAtkPotionActive)
                {
                    controller.GainAttackBoost(item.AttackPercent);
                }
                else if (item.name.Contains("Sprint Potion") && !controller.IsSpeedPotionActive)
                {
                    controller.GainMovementBoost(item.MovementSpdPercent);
                }
            }

            return item;
        }

        return null;
    }
}
