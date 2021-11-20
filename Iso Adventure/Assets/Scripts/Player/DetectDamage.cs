using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamage : MonoBehaviour
{
    public GameObject parent;
    public IEnemyStateMachine machine;

    private void OnEnable()
    {
        machine = parent.GetComponent<IEnemyStateMachine>();
    }

    private void OnTriggerStay(Collider other)
    {

        PlayerController controller = other.GetComponent<PlayerController>();
        if (!controller) return;

        if (controller.machine.parrying && machine.Parryable)
        {
            controller.health.Invulnerability(1);
            machine.Stagger();
            machine.Controller.DeactivateAttack();
        }
        else if (!controller.invuln)
        {
            controller.health.TakeDamage(machine.Stats.damage);
        }
    }
}
