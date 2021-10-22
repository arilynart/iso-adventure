using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


namespace Arilyn.State.EnemyState.Golem
{
    public class AttackState : EnemyState
    {
        public AttackState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            if (machine.Stats.activeAttack == machine.Stats.attacks[2])
            {
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.Transform.GetComponent<BullAttack>().Jumping());
                machine.Agent.SetDestination(DeveloperConsoleBehavior.PLAYER.transform.position);
            }
            else
            {
                machine.Agent.acceleration = 0;
                machine.Agent.speed = 0;
                machine.Agent.SetDestination(machine.Transform.position);
            }

            machine.Animator.Play(machine.Stats.animationName);
            DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.Controller.AttackAnimation(machine.Stats.boxStart, machine.Stats.boxEnd));
            yield return null;
        }

        public override IEnumerator ExitState()
        {
            Debug.Log("Golem: Exiting attack state");
            if (machine.Stats.activeAttack == machine.Stats.attacks[0] || machine.Stats.activeAttack == machine.Stats.lockedAttacks[0])
            {
                Debug.Log("Golem: Attack 1 or 2 detected.");
                machine.Agent.acceleration = 75;
                machine.Agent.speed = 15;
            }
            else
            {
                Debug.Log("Golem: Other attack detected.");
                machine.Agent.acceleration = machine.Acceleration;
                machine.Agent.speed = machine.Speed;
                if (machine.Stats.activeAttack == machine.Stats.lockedAttacks[2])
                {
                    machine.Transform.GetComponent<BullAttack>().ActivateSlows();
                }

            }

            machine.Stats.NextAttack();
            yield return null;
        }
    }
}
