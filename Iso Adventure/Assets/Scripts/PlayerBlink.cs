using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlink : MonoBehaviour
{
    public BoxCollider blinkCollider;

    public void Blink()
    {

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
