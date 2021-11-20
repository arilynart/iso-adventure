using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Soldier
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

            yield return new WaitForSeconds(machine.StaggerDuration);

            machine.Agent.speed = machine.Speed;
            machine.Agent.acceleration = machine.Acceleration;
            machine.Animator.ResetTrigger("Stagger");
            machine.Animator.SetTrigger("ReturnStagger");
            machine.Controller.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
        }
    }
}
