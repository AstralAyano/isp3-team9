using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLobbyManager : MonoBehaviour
{
    [SerializeField]
    private SceneLoader sceneLoader;

    [SerializeField]
    private ScriptablePlayerStats playerStats;

    public static bool doorTouched = false;

    private void Start()
    {
        sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (playerStats.chosenClass == ScriptablePlayerStats.playerClass.None)
        {
            doorTouched = true;
            DialogueManager.instance.TriggerDialogue(DialogueManager.npcType.Door);
            return;
        }

        doorTouched = false;
        if (other.gameObject.CompareTag("Player"))
        {
            sceneLoader.LoadScene("SceneLevel");
        }
    }
}
