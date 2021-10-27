using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamage : MonoBehaviour
{
    PlayerController controller;

    private void Start()
    {
        controller = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other) return;
        //if (!GetComponent<EnemyStats>()) return;
        if (!controller) return;

        if (other.tag == "Attack" && !controller.invuln)
        {
            EnemyStats stats = other.transform.parent.GetComponent<EnemyStats>();
            if (!stats) return;

            controller.health.TakeDamage(stats.damage);
        }
    }
}
