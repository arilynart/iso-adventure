using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorTrigger : MonoBehaviour
{
    int normalMask;
    Color skyColor;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;
        normalMask = Camera.main.cullingMask;

        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Indoors") | 1 << LayerMask.NameToLayer("Player"));
        skyColor = Camera.main.backgroundColor;
        Camera.main.backgroundColor = Color.black;
    }

    private void OnTriggerExit(Collider other)
    {
        Camera.main.cullingMask = normalMask;
        Camera.main.backgroundColor = skyColor;
    }
}
