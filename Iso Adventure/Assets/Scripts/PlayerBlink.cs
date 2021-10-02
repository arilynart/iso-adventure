using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlink : MonoBehaviour
{
    PlayerController controller;
    PlayerDodge playerDodge;
    public float blinkDistance = 100f;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        playerDodge = GetComponent<PlayerDodge>();
    }

    public void Blink()
    {
        //Vector3 targetPosition = controller.point * blinkDistance;

        //transform.position += targetPosition;

        playerDodge.dashSpeed = blinkDistance;
    }
}
