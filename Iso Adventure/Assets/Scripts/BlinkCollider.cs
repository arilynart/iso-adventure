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

    public void OnTriggerStay(Collider other)
    {
        if (!GetComponent<BlinkCollider>()) return;
        Debug.Log("Triggered collision");

        //shoot laser from behind us, if we hit a blink point
        if (Physics.Raycast(backPoint.position, transform.parent.GetComponent<PlayerController>().point, out hitInfo, 9f, blink))
        {
            Debug.Log("Raycast hit");
            if (hitInfo.collider.tag == "Blink")
            {
                
                //stop dash movement
                transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //teleport us there
                transform.parent.position = hitInfo.collider.transform.position;
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
