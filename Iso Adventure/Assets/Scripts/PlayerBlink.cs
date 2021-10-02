using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlink : MonoBehaviour
{
    PlayerController controller;
    public float blinkDistance = 2f;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void Blink()
    {
        Vector3 targetPosition = new Vector3(blinkDistance * controller.point.x, transform.position.y, blinkDistance * controller.point.z);

        transform.position = targetPosition;
    }
}
