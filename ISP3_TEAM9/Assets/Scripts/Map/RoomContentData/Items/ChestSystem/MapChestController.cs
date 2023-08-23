using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChestController : MonoBehaviour
{
    [SerializeField] RandomItemList itemList;
    private bool playerWithinRange = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerWithinRange)
        {
            Instantiate(itemList.GetRandomItem(), transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerWithinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerWithinRange = false;
        }
    }
}
