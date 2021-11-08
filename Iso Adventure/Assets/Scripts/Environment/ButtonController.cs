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
        if (other.tag != "Interact") return;
        //enabled = true;
        dialogueSystem.EnterRangeOfInteractable(transform);
        //dialogueSystem.GateButton();

        PlayerController controller = other.transform.parent.GetComponent<PlayerController>();
        if (controller.interactTrigger)
        {
            //reset interact trigger
            controller.interactTrigger = false;

            CameraFollow.MAINCAMERA.GetComponent<Cutscene>().CutsceneStart(gate.transform, 3f);
            dialogueSystem.DropDialogue();
            area.enabled = false;
            StartCoroutine(GateDelay());
            if (gate.layer == LayerMask.NameToLayer("Indoors"))
            { 
                Debug.Log("Target is indoors.");
                CameraFollow.MAINCAMERA.cullingMask = 1 << LayerMask.NameToLayer("Indoors");
                CameraFollow.MAINCAMERA.backgroundColor = Color.black;
            }
        }
    }

    IEnumerator GateDelay()
    {
        yield return new WaitForSeconds(1);
        gate.GetComponent<GateController>().Open();
    }
    private void OnTriggerExit(Collider other)
    {
        dialogueSystem.DropDialogue();
    }
}

