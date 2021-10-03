using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using Bolt;

public class BlinkCollider : MonoBehaviour
{
    RaycastHit hitInfo;
    public LayerMask blink;
    public Transform backPoint;
    PlayerController controller;
    PlayerBlink playerBlink;

    private void Start()
    {
        controller = transform.parent.GetComponent<PlayerController>();
        playerBlink = transform.parent.GetComponent<PlayerBlink>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (!GetComponent<BlinkCollider>()) return;
        Debug.Log("Triggered collision");
        if (playerBlink.mouseBlink)
        {
            Debug.Log("Mouse Blink");
            transform.rotation = controller.mousePoint.transform.rotation;
            if (Physics.Raycast(backPoint.position, controller.mousePoint.transform.forward, out hitInfo, 9f, blink))
            {
                Debug.Log("Raycast hit");
                if (hitInfo.collider.tag == "Blink")
                {
                    Blink();
                }
            }
        }
        else
        {
            Debug.Log("Point Blink");
            //shoot laser from behind us, if we hit a blink point
            if (Physics.Raycast(backPoint.position, transform.parent.GetComponent<PlayerController>().point, out hitInfo, 9f, blink))
            {
                Debug.Log("Raycast hit");
                if (hitInfo.collider.tag == "Blink")
                {
                    Blink();
                }
            }
        }
        transform.rotation = transform.parent.rotation;
    }

    void Blink()
    {
        //stop dash movement
        transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //teleport us there
        transform.parent.position = hitInfo.collider.transform.position;
        GetComponent<BoxCollider>().enabled = false;
    }
}
