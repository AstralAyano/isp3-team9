using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static float timer = 0f; //Record game time after start button is pressed and before the game ends
    public static bool startClicked = false;
    private float passedTime = 0f; //Record game time before start button is pressed

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (startClicked)
        {
            timer = Time.realtimeSinceStartup - passedTime;
        }
        else
        {
            passedTime = Time.realtimeSinceStartup;
            timer = 0f;
        }
        Debug.Log("Timer: " + (int)timer);
    }
}
