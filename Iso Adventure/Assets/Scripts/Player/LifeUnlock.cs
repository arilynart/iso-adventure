using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LifeUnlock : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    private HealthBar healthBar;

    string[] dialogueLines = new string[1];

    string NPCName = "";

    public static int UNLOCKS = 0;
    public static int REQUIRED_UNLOCKS = 2;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        healthBar = FindObjectOfType<HealthBar>();
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (!other) return;
        if (!other.GetComponent<PlayerController>()) return;
        if (other.gameObject.tag != "Player") return;
        //enabled = true;
        dialogueSystem.EnterRangeOfInteractable(transform);

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller.interactTrigger)
        {
            controller.interactTrigger = false;
            dialogueSystem.SetController(controller);
            enabled = true;
            dialogueSystem.npcName = NPCName;
            SetDialogue();
            dialogueSystem.dialogueLines = dialogueLines;
            dialogueSystem.NPCName();
            Destroy(gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        dialogueSystem.OutOfRange();
        enabled = false;
    }

    void SetDialogue()
    {
        UNLOCKS++;
        dialogueLines[0] = Dialogue();
    }


    string Dialogue()
    {
        string part2;
        if (UNLOCKS < REQUIRED_UNLOCKS)
        {
            part2 = ("Find " + (REQUIRED_UNLOCKS - UNLOCKS) + " more cluster to increase your life essence.");
        }
        else
        {
            part2 = "Your life essence is increased!";
            UNLOCKS -= REQUIRED_UNLOCKS;
            PlayerHealth.LIFE_UNLOCKED++;
            healthBar.ResetBar();
        }

        return ("You have found a focused cluster of spirit. " + part2);
    }
}
