using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float timer = 0f;
    public static bool startClicked = false;

    private void Awake()
    {
        DontDestroyOnLoadManager.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (startClicked)
        {
            timer += Time.deltaTime;
        }
    }
}
