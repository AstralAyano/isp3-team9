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
                if (invManager.lastItemAdded == itemsToPickup[7])
                {
                    invManager.lastItemAdded.health = 0;
                    invManager.lastItemAdded.defense = 0;
                    invManager.lastItemAdded.attack = 0;
                    invManager.lastItemAdded.attackSpeed = 0;
                    invManager.lastItemAdded.moveSpeed = 0;
                    invManager.lastItemAdded.projectileSpeed = 0;

                    int randIncrease = Random.Range(0, 10);

                    Debug.Log(randIncrease);

                    if (randIncrease < 4)
                    {
                        int randStatI = Random.Range(0, 6);

                        switch (randStatI)
                        {
                            case 0:
                                invManager.lastItemAdded.health = 1;
                                break;
                            case 1:
                                invManager.lastItemAdded.defense = 1;
                                break;
                            case 2:
                                invManager.lastItemAdded.attack = 1;
                                break;
                            case 3:
                                invManager.lastItemAdded.attackSpeed = 1;
                                break;
                            case 4:
                                invManager.lastItemAdded.moveSpeed = 1;
                                break;
                            case 5:
                                invManager.lastItemAdded.projectileSpeed = 1;
                                break;
                        }

                        int randDecrease = Random.Range(0, 10);

                        if (randDecrease < 2)
                        {
                            int randStatD = Random.Range(0, 6);

                            switch (randStatD)
                            {
                                case 0:
                                    invManager.lastItemAdded.health = -1;
                                    break;
                                case 1:
                                    invManager.lastItemAdded.defense = -1;
                                    break;
                                case 2:
                                    invManager.lastItemAdded.attack = -1;
                                    break;
                                case 3:
                                    invManager.lastItemAdded.attackSpeed = -1;
                                    break;
                                case 4:
                                    invManager.lastItemAdded.moveSpeed = -1;
                                    break;
                                case 5:
                                    invManager.lastItemAdded.projectileSpeed = -1;
                                    break;
                            }
                        }
                    }
                }
                //gameObject.GetComponentInParent<PlayerController>().playSFX(11);
                Destroy(other.gameObject);
            }
        }
    }
}
