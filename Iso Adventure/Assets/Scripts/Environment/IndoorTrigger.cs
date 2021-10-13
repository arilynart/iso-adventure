using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorTrigger : MonoBehaviour
{
    public static bool INDOORS = false;

    int normalMask;
    Color skyColor;

    private void Start()
    {
        normalMask = Camera.main.cullingMask;
        skyColor = Camera.main.backgroundColor;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        if (!FadeToBlack.FADEAWAY) {
            CameraFollow.LOCKTARGET = transform;
            CameraFollow.LOCK = true;
            Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Indoors") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Indoor Enemy") | 1 << LayerMask.NameToLayer("Indoor Ragdolls"));
            Camera.main.backgroundColor = Color.black;
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
            CameraFollow.LOCK = false;
            Camera.main.cullingMask = normalMask;
            Camera.main.backgroundColor = skyColor;
        }

    }
}
