using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLobbyManager : MonoBehaviour
{
    private SceneLoader sceneLoader;

    [SerializeField]
    private ScriptablePlayerStats playerStats;

    public static bool doorTouched = false;

    private void Start()
    {
        try
        {
            sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
        }
        catch (Exception)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (playerStats.chosenClass == ScriptablePlayerStats.playerClass.None)
        {
            doorTouched = true;
            DialogueManager dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            dialogueManager.TriggerDialogue(DialogueManager.npcType.Door);
            return;
        }

        doorTouched = false;
        if (other.gameObject.CompareTag("Player"))
        {
            sceneLoader.LoadScene("SceneLevel");
        }
    }
}
