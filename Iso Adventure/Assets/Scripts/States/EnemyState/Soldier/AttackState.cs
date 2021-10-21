using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arilyn.State.EnemyState.Soldier
{
    public class AttackState : EnemyState
    {
        public AttackState(EnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.agent.acceleration = 0;
            machine.agent.speed = 0;
            machine.agent.SetDestination(machine.transform.position);

            machine.animator.Play(machine.stats.animationName);
            machine.StartCoroutine(machine.controller.AttackAnimation(machine.stats.boxStart, machine.stats.boxEnd));
            yield break;
        }

        public override IEnumerator ExitState()
        {
            machine.agent.acceleration = 8;
            machine.agent.speed = 2.6f;
            machine.stats.NextAttack();
            yield break;
        }
    }
}
