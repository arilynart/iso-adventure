using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamage : MonoBehaviour
{
    public GameObject parent;
    public EnemyStats stats;

    private void OnEnable()
    {
        stats = parent.GetComponent<EnemyStats>();
    }

    private void OnTriggerStay(Collider other)
    {
        //if (!GetComponent<EnemyStats>()) return;
        if (!stats) return;
        PlayerController controller = other.GetComponent<PlayerController>();
        if (!controller) return;
        if (!controller.invuln)
        {
            controller.health.TakeDamage(stats.damage);
        }
    }
}
