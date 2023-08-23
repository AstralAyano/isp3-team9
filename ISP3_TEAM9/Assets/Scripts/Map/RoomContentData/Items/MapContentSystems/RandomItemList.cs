using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemList : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemList;

    public GameObject GetRandomItem()
    {
        GameObject randomizedItem;

        randomizedItem = itemList[UnityEngine.Random.Range(0, itemList.Count - 1)];

        return randomizedItem;
    }
}
