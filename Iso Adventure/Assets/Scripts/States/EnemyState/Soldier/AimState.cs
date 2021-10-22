using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Soldier
{
    public class AimState : EnemyState
    {
        public AimState(IEnemyStateMachine mch) : base(mch) { }

        bool executed = false;

        public override IEnumerator EnterState()
        {
            machine.Agent.speed = 5;
            yield break;
        }

        public override void LocalUpdate()
        {
            if (machine.AttackDistance <= machine.Stats.range && !executed)
            {
                //Vector3 targetRot = Vector3.RotateTowards(machine.transform.forward, machine.lookRotation, 4 * Time.deltaTime, 0);
                Quaternion lookRotation = Quaternion.LookRotation(machine.LookRotation);
                machine.Transform.rotation = Quaternion.Slerp(machine.Transform.rotation, lookRotation, 5 * Time.deltaTime);
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new AttackState(machine)));
                executed = true;
            }
            else
            {
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
            }
        }
    }
}
