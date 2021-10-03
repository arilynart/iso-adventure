using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private DialogueSystem dialogueSystem;

    public GameObject gate;
    public BoxCollider area;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other) return;
        if (!other.GetComponent<PlayerController>()) return;
        //enabled = true;
        dialogueSystem.EnterRangeOfInteractable(transform);
        //dialogueSystem.GateButton();

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller.interactTrigger)
        {
            gate.GetComponent<GateController>().Open();
            dialogueSystem.DropDialogue();
            area.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        dialogueSystem.DropDialogue();
    }
}

