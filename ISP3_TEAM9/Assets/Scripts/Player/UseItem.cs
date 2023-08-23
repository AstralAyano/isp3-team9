using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            UseSelectedItem();
        }
        if (Input.GetKeyDown("2"))
        {
            UseSelectedItem();
        }
        if (Input.GetKeyDown("3"))
        {
            UseSelectedItem();
        }
    }

    public Item GetSelectedItem()
    {

    }
}
