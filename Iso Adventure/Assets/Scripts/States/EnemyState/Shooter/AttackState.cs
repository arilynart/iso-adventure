using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


namespace Arilyn.State.EnemyState.Shooter
{
    public class AttackState : EnemyState
    {
        public AttackState(IEnemyStateMachine mch) : base(mch) { }


        public override IEnumerator EnterState()
        {
            if (machine.Agent.enabled) machine.Agent.SetDestination(machine.Transform.position);
            machine.Agent.acceleration = 0;
            machine.Agent.speed = 0;
            machine.Attack();
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


    }
}
