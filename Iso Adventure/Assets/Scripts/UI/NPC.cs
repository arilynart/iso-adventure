using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NPC : MonoBehaviour
{
    private DialogueSystem dialogueSystem;

    public string npcName;

    [TextArea(5,10)]
    public string[] sentences;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (!other) return;
        if (!other.GetComponent<PlayerController>()) return;
        if (other.gameObject.tag != "Player") return;
        //enabled = true;
        dialogueSystem.EnterRangeOfNPC(transform);
        
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller.interactTrigger)
        {
            dialogueSystem.SetController(controller);
            enabled = true;
            dialogueSystem.npcName = npcName;
            dialogueSystem.dialogueLines = sentences;
            dialogueSystem.NPCName();
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        dialogueSystem.OutOfRange();
        enabled = false;
    }
}
