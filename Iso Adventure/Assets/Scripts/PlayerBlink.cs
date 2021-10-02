using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ludiq;
using Bolt;

public class PlayerBlink : MonoBehaviour
{
    public BoxCollider blinkCollider;

    public bool blinkable;

    public void Blink()
    {
        if (!blinkable) return;
        Debug.Log("Starting Blink");
        blinkCollider.enabled = true;
        //Vector3 targetPosition = controller.point * blinkDistance;

        //transform.position += targetPosition;
        StartCoroutine(BlinkDisable());

    }

    IEnumerator BlinkDisable()
    {
        yield return new WaitForSeconds(0.2f);

        blinkCollider.enabled = false;
    }
}
