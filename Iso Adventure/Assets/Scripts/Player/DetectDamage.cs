using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamage : MonoBehaviour
{
    public GameObject parent;
    public IEnemyStateMachine machine;
    PlayerController controller;
    bool triggered;

    private void OnEnable()
    {
        machine = parent.GetComponent<IEnemyStateMachine>();
        triggered = false;
    }

    private void OnTriggerStay(Collider other)
    {

        PlayerController pc = other.GetComponent<PlayerController>();
        if (!pc || triggered) return;
        controller = pc;
        triggered = true;

        StartCoroutine(ParryTime());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (!pc || !triggered) return;

        triggered = false;
    }
    IEnumerator ParryTime()
    {
        Debug.Log(this + " Starting Parry Coroutine");
        float time = 0;
        while (time < controller.machine.parryDuration)
        {
            //is the attack parryable?
            if (machine.Parryable)
            {
                //every frame we check if the player is parrying
                if (controller.machine.parrying)
                {
                    //if yes, are we facing the target?
                    float dot = Vector3.Dot(machine.LookRotation, controller.transform.forward);
                    Debug.Log(this + " Dot: " + dot);
                    if (dot < 0f)
                    {
                        //parry
                        Debug.Log(this + " Player is facing forward");
                        StartCoroutine(controller.health.Invulnerability(1));
                        machine.Stagger();
                        machine.Controller.DeactivateAttack();
                        break;
                    }
                }
            }
            else break;
            time += Time.deltaTime;
            yield return null;
        }
        //after parry, check if we're invuln. otherwise take damage.
        if (!controller.invuln) controller.health.TakeDamage(machine.Stats.damage);
        triggered = false;
    }
}
