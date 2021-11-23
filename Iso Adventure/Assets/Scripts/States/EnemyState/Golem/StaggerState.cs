using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


namespace Arilyn.State.EnemyState.Golem
{
    public class StaggerState : EnemyState
    {
        public StaggerState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            Debug.Log("Entered Stagger State");
            machine.Agent.speed = 0f;
            machine.Agent.acceleration = 0f;
            machine.Animator.ResetTrigger("ReturnStagger");
            machine.Animator.SetTrigger("Stagger");
            while (StaggerGauge.STAGGER > 0)
            {
                StaggerGauge.ADD_STAGGER(-(1 / machine.StaggerDuration) * Time.deltaTime);
                yield return null;
            }

            machine.Agent.speed = machine.Speed;
            machine.Agent.acceleration = machine.Acceleration;
            machine.Animator.ResetTrigger("Stagger");
            machine.Animator.SetTrigger("ReturnStagger");
            machine.Controller.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
            StaggerGauge.RESET_STAGGER();
        }
    }
}
