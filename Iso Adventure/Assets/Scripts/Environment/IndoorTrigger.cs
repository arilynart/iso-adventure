using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorTrigger : MonoBehaviour
{
    public static bool INDOORS = false;
    public bool cameraLock;

    int normalMask;
    Color skyColor;

    private void Start()
    {
        normalMask = CameraFollow.MAINCAMERA.cullingMask;
        skyColor = CameraFollow.MAINCAMERA.backgroundColor;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        if (!FadeToBlack.FADEAWAY) {
            if (cameraLock)
            {
                CameraFollow.LOCKTARGET = transform;
                CameraFollow.LOCK = true;
            }
            CameraFollow.MAINCAMERA.cullingMask = (1 << LayerMask.NameToLayer("Indoors") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Indoor Enemy") | 1 << LayerMask.NameToLayer("Indoor Ragdolls"));
            CameraFollow.MAINCAMERA.backgroundColor = Color.black;
            INDOORS = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        FadeToBlack.FADEOUT();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        FadeToBlack.FADEOUT();
        INDOORS = false;

        StartCoroutine(FadeDelay());
    }

    public IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (!INDOORS)
        {
            if (cameraLock)
                CameraFollow.LOCK = false;

            CameraFollow.MAINCAMERA.cullingMask = normalMask;
            CameraFollow.MAINCAMERA.backgroundColor = skyColor;
        }

    }
}
