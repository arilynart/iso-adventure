using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Enemy" && !transform.parent.gameObject.GetComponent<PlayerController>().invuln)
        {
            transform.parent.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
