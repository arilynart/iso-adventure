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
            machine.Agent.acceleration = 0;
            machine.Agent.speed = 0;
            if (machine.Agent.enabled) machine.Agent.SetDestination(machine.Transform.position);

            machine.Animator.Play(machine.Stats.animationName);
            DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.Controller.AttackAnimation(machine.Stats.boxStart, machine.Stats.boxEnd));
            yield break;
        }

        public override IEnumerator ExitState()
        {
            machine.Agent.acceleration = machine.Acceleration;
            machine.Agent.speed = machine.Speed;
            machine.Stats.NextAttack();
            yield break;
        }
    }
}
