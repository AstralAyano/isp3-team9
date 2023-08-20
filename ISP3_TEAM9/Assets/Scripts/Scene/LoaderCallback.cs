using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    public static bool firstUpdate;

    private void Awake()
    {
        firstUpdate = true;
    }

    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            SceneLoader.LoaderCallback();
        }
    }
}
