using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject inventoryBar;

    [Header("Parent Objects")]
    [SerializeField] private GameObject bookUI;
    [SerializeField] private GameObject hudUI;

    [Header("Bar New Local Positions")]
    [SerializeField] private Vector2 bookUIBarPos;
    [SerializeField] private Vector2 hudUIBarPos;

    public void OpenInventory()
    {
        Time.timeScale = 0;

        inventoryBar.gameObject.transform.SetParent(bookUI.transform);
        inventoryBar.transform.localPosition = bookUIBarPos;
    }

    public void CloseInventory()
    {
        Time.timeScale = 1.0f;

        inventoryBar.gameObject.transform.SetParent(hudUI.transform);
        inventoryBar.transform.localPosition = hudUIBarPos;
    }
}
