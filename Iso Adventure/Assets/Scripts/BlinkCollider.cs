using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkCollider : MonoBehaviour
{
    RaycastHit hitInfo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Blink")
        {
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 0.5f))
            transform.parent.position = other.ClosestPoint(transform.position) + new Vector3(0, 0.5f, 0);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
