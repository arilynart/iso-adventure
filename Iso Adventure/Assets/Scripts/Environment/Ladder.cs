using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Transform interactPoint;

    private DialogueSystem dialogueSystem;
    bool playerPresent;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        if (other.GetComponent<PlayerController>().onLadder) return;

        playerPresent = true;
        dialogueSystem.EnterRangeOfInteractable(interactPoint);
    }

    private void OnTriggerExit(Collider other)
    {
        dialogueSystem.DropDialogue();
    }
}
