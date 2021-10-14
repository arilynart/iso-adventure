using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ludiq;
using Bolt;
public class DialogueSystem : MonoBehaviour
{
    PlayerController controller;

    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueBox;
    public GameObject interactable;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.1f;

    int currentDialogueIndex = 0;

    public string npcName;

    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;
    bool interacting = false;
    bool dialoguePause = false;

    Transform target;


    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = "";
    }

    private void Update()
    {
        if (interacting && target != null)
        {
            interactable.transform.position = CameraFollow.MAINCAMERA.WorldToScreenPoint(target.position);
        }
    }

    public void EnterRangeOfInteractable(Transform npc)
    {
        outOfRange = false;
        target = npc;
        
        interactable.SetActive(true);
        interacting = true;

        if(dialogueActive == true)
        {
            interactable.SetActive(false);
            interacting = false;
        } 
        
    }

    public void NPCName()
    {
        if(controller.interacting && !dialogueActive)
        {
                dialogueBox.SetActive(true);
                nameText.text = npcName;
                dialogueActive = true;
                StartCoroutine(StartDialogue());
        }
    }

    IEnumerator StartDialogue()
    {
        Variables.Object(controller.gameObject).Set("animLock", true);
        controller.moving = false;
        if (outOfRange) yield break;
        Debug.Log("Dialogue Pressed " + dialogueLines.Length);

        int dialogueLength = dialogueLines.Length;
        currentDialogueIndex = 0;

        //if the current sentence is less than the maximum sentence
        while (currentDialogueIndex < dialogueLength)
        {
            Debug.Log("Dialogue Index: " + currentDialogueIndex);
            //if we're not multiplying
            if (!letterIsMultiplied && !dialoguePause)
            {
                Debug.Log("Starting Dialogue");
                //multiply
                letterIsMultiplied = true;
                StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex]));
                
                yield return null;

            }
            else
            {
                yield return null;
            }

        }
        dialogueEnded = true;
        Variables.Object(controller.gameObject).Set("animLock", false);
        /*
                while (true)
                {
                    if (controller.interacting) break;
                    else yield return null;
                }*/
        //dialogueEnded = false;
        dialogueActive = false;
        DropDialogue();
    }

    IEnumerator DisplayString(string s)
    {
        Debug.Log("Displaying Dialogue: " + s);
        if (outOfRange) yield break;

        int stringLength = s.Length;
        int currentCharacter = 0;

        dialogueText.text = "";

        while (currentCharacter < stringLength)
        {
            dialogueText.text += s[currentCharacter];
            currentCharacter++;

            if (controller.interacting)
            {
                yield return new WaitForSeconds(letterDelay * letterMultiplier);
            }
            else
            {
                yield return new WaitForSeconds(letterDelay);

            }
        }
        
        dialogueEnded = true;
        controller.interactTrigger = false;
        while (true)
        {
            if (controller.interactTrigger)
            {
                controller.interactTrigger = false;
                break;
            }
            else yield return null;
        }
        currentDialogueIndex++;
        dialogueEnded = false;
        letterIsMultiplied = false;
        dialogueText.text = "";
    }

    public IEnumerator WaitForInteract()
    {
        dialoguePause = true;
        controller.interactTrigger = false;
        while (dialoguePause)
        {
            if (controller.interactTrigger)
                controller.interactTrigger = false;
                dialoguePause = false;
            yield return null;
        }

    }

    public void DropDialogue()
    {
        interactable.SetActive(false);
        dialogueBox.SetActive(false);
        interacting = false;
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            DropDialogue();
        }
    }

    public void SetController(PlayerController c)
    {
        controller = c;
    }

}
