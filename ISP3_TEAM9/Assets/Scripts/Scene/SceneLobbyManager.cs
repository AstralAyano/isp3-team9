using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLobbyManager : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sceneLoader.LoadScene("SceneLevel");
        }
    }
}
