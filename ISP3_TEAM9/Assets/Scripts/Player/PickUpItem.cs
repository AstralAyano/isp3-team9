using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("Object")]
    public InventoryManager invManager;

    [Header("List of Scriptable Objects")]
    public Item[] itemsToPickup;
    public string[] itemsName;

    void Start()
    {
        invManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name + " : " + gameObject.name + " : " + Time.time);

        for (int i = 0; i < itemsToPickup.Length; i++)
        {
            if (other.gameObject.name.Contains(itemsName[i]) && invManager.AddItem(itemsToPickup[i]))
            {
                //gameObject.GetComponentInParent<PlayerController>().playSFX(11);
                Destroy(other.gameObject);
            }
        }
    }
}
