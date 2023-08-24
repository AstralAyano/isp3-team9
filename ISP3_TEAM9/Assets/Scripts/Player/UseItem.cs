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
        /*if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if (isNumber && number > 0 && number < 4)
            {
                UseSelectedItem(true);
            }
        }*/
    }

    public void UseSelectedItem(bool consumable)
    {
        Item usingItem = invManager.GetSelectedItem(true, controller.IsHealthMax, controller.IsSpeedPotionActive, controller.IsDefensePotionActive, controller.IsAtkPotionActive, controller.IsAtkSpdPotionActive);
        //InventorySlot slot = invSlots[selectedSlot];
        //InventoryItem itemSlot = slot.GetComponentInChildren<InventoryItem>();

        if (usingItem != null)
        {
            if (consumable)
            {
                if (usingItem.itemName.Contains("Small Health Potion") && !controller.IsHealthMax)
                {
                    controller.GainHP(usingItem.HealPercent);
                }
                else if (usingItem.itemName.Contains("Big Health Potion") && !controller.IsHealthMax)
                {
                    controller.GainHP(usingItem.HealPercent);
                }
                else if (usingItem.itemName.Contains("Defense Potion") && !controller.IsDefensePotionActive)
                {
                    controller.GainDefenseBoost(usingItem.DefensePercent);
                }
                else if (usingItem.itemName.Contains("Attack Speed Potion") && !controller.IsAtkSpdPotionActive)
                {
                    controller.GainAtkSpdBoost(usingItem.AttackSpdPercent);
                }
                else if (usingItem.itemName.Contains("Attack Potion") && !controller.IsAtkPotionActive)
                {
                    controller.GainAttackBoost(usingItem.AttackPercent);
                }
                else if (usingItem.itemName.Contains("Sprint Potion") && !controller.IsSpeedPotionActive)
                {
                    controller.GainMovementBoost(usingItem.MovementSpdPercent);
                }
            }
        }
    }
}
