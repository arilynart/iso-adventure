using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorTrigger : MonoBehaviour
{
    public static bool INDOORS = false;

    int normalMask;
    Color skyColor;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;
        normalMask = Camera.main.cullingMask;

        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Indoors") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Indoor Enemy") | 1 << LayerMask.NameToLayer("Indoor Ragdolls"));
        skyColor = Camera.main.backgroundColor;
        Camera.main.backgroundColor = Color.black;
        INDOORS = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        Camera.main.cullingMask = normalMask;
        Camera.main.backgroundColor = skyColor;

        INDOORS = false;
    }
}
