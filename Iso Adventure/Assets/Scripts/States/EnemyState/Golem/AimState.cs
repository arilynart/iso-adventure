using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Golem
{
    public class AimState : EnemyState
    {
        public AimState(IEnemyStateMachine mch) : base(mch) { }


        public override IEnumerator EnterState()
        {
            machine.Agent.speed = 5;
            yield return null;
        }

        public override void LocalUpdate()
        {
            if (machine.AttackDistance <= machine.Stats.range)
            {
                //Vector3 targetRot = Vector3.RotateTowards(machine.transform.forward, machine.lookRotation, 4 * Time.deltaTime, 0);
                Quaternion lookRotation = Quaternion.LookRotation(machine.LookRotation);
                machine.Transform.rotation = Quaternion.Slerp(machine.Transform.rotation, lookRotation, 5 * Time.deltaTime);
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new AttackState(machine)));
            }
            else
            {
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
            }
        }
    }
}
