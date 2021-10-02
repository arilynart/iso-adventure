using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlink : MonoBehaviour
{
    PlayerController controller;
    PlayerDodge playerDodge;
    public BoxCollider blinkCollider;

    private void Start()
    {
        controller = transform.parent.GetComponent<PlayerController>();
        playerDodge = transform.parent.GetComponent<PlayerDodge>();
    }

    public void Blink(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            blinkCollider.enabled = true;
            //Vector3 targetPosition = controller.point * blinkDistance;

            //transform.position += targetPosition;
            StartCoroutine(BlinkDisable());
        }
    }

    IEnumerator BlinkDisable()
    {

        yield return new WaitForSeconds(0.1f);

        blinkCollider.enabled = false;
    }
}
