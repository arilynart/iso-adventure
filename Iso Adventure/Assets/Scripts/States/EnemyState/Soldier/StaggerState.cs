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
            machine.Agent.speed = 0f;
            machine.Agent.acceleration = 0f;
            machine.Animator.SetTrigger("Stagger");
            float time = 0;
            while (time < machine.StaggerDuration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            machine.Agent.speed = machine.Speed;
            machine.Agent.acceleration = machine.Acceleration;
            machine.Animator.ResetTrigger("Stagger");
            machine.ChangeState(new ChaseState(machine));
        }
    }
}
