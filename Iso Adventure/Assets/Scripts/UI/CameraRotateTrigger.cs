using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateTrigger : MonoBehaviour
{
    public int state;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        StartCoroutine(CameraFollow.MAINCAMERA.GetComponent<CameraRotate>().Rotation(state));
    }
}
