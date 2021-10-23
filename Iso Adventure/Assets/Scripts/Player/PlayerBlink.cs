using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlink : MonoBehaviour
{

    public BoxCollider blinkCollider;
    PlayerController controller;

    public bool mouseBlink = false;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void Blink()
    {

        if (controller.MouseActivityCheck())
            mouseBlink = true;

        blinkCollider.enabled = true;
        //Vector3 targetPosition = controller.point * blinkDistance;

        //transform.position += targetPosition;
        StartCoroutine(BlinkDisable());
    }

    IEnumerator BlinkDisable()
    {
        yield return new WaitForSeconds(0.2f);

        blinkCollider.enabled = false;
        mouseBlink = false;
    }
}
