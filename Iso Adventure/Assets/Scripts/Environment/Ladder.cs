using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class Ladder : MonoBehaviour
{
    public Transform interactPoint;
    public Transform startPosition;
    public Transform LookPoint;

    private DialogueSystem dialogueSystem;
    bool playerPresent;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    private void Update()
    {
        if (playerPresent && DeveloperConsoleBehavior.PLAYER.interactTrigger) 
        {
            DeveloperConsoleBehavior.PLAYER.ClimbLadder(startPosition.position, this);
            playerPresent = false;
        }
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
        playerPresent = false;
    }
}
