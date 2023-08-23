using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLadderController : MonoBehaviour
{
    private bool playerWithinRange = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerWithinRange)
        {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();

            // swap scene
            sceneLoader.LoadScene("SceneLevel");
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
