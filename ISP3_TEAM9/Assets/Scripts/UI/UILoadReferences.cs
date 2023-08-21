using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadReferences : MonoBehaviour
{
    void Start()
    {
        try
        {
            Canvas[] uiCanvas = GameObject.FindWithTag("UI").GetComponentsInChildren<Canvas>(true);

            for (int i = 0; i < 3; i++)
            {
                uiCanvas[i].worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            }

            UIBookController uiBook = GameObject.FindWithTag("UI").GetComponentInChildren<UIBookController>(true);
            UIHUDController uiHUD = GameObject.FindWithTag("UI").GetComponentInChildren<UIHUDController>(true);
        
            uiBook.playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            uiHUD.playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
