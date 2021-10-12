using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateTrigger : MonoBehaviour
{
    public int state;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        StartCoroutine(Camera.main.GetComponent<CameraRotate>().Rotation(state));
    }
}
