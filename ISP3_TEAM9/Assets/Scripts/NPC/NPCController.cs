using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private DialogueManager.npcType npcName;

    public void Interact()
    {
        DialogueManager.instance.TriggerDialogue(npcName);
    }
}
