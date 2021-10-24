using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


namespace Arilyn.State.EnemyState.Soldier
{
    public class AttackState : EnemyState
    {
        public AttackState(IEnemyStateMachine mch) : base(mch) { }


        public override IEnumerator EnterState()
        {
            machine.Animator.SetTrigger(machine.Stats.animationName);
            DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.Controller.AttackAnimation(machine.Stats.boxStart, machine.Stats.boxEnd));
            DeveloperConsoleBehavior.PLAYER.StartCoroutine(StopDelay(machine.Stats.activeAttack.boxStart));
            yield break;
        }

        public override IEnumerator ExitState()
        {
            machine.Agent.acceleration = machine.Acceleration;
            machine.Agent.speed = machine.Speed;
            machine.Animator.ResetTrigger(machine.Stats.animationName);
            machine.Stats.NextAttack();
            yield break;
        }

        IEnumerator StopDelay(float duration)
        {
            if (machine.Agent.enabled) machine.Agent.SetDestination(machine.Transform.position);
            machine.Agent.acceleration = 0;
            machine.Agent.speed = 0;
            float time = 0;
            while (time < duration)
            {
                Quaternion lookRotation = Quaternion.LookRotation(machine.LookRotation);
                machine.Transform.rotation = Quaternion.Slerp(machine.Transform.rotation, lookRotation, 5 * Time.deltaTime);
                time += Time.deltaTime;
                yield return null;
            }

        }
    }
}
