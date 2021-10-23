using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


namespace Arilyn.State.EnemyState.Golem
{
    public class WanderState : EnemyState
    {
        public WanderState(IEnemyStateMachine mch) : base(mch) { }

        bool toggle = false;

        public override IEnumerator EnterState()
        {
            machine.Agent.speed = machine.Speed;
            machine.Agent.acceleration = machine.Acceleration;
            AnimatorStateInfo info = machine.Animator.GetCurrentAnimatorStateInfo(0);
            float normal = info.normalizedTime;
            if (normal != 0)
            {
                machine.Animator.Play("idle");
            }
            yield break;
        }

        public override void LocalUpdate()
        {
            if (machine.CanSeePlayer && !toggle)
            {
                Debug.Log("Golem: Seen player, changing to chase");
                //change state to chase
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
                toggle = true;
            }
        }
    }
}
