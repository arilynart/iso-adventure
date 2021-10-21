using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.EnemyState.Soldier
{
    public class AimState : EnemyState
    {
        public AimState(EnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.agent.speed = 5;
            yield break;
        }

        public override void LocalUpdate()
        {
            if (machine.attackDistance <= machine.stats.range)
            {
                //Vector3 targetRot = Vector3.RotateTowards(machine.transform.forward, machine.lookRotation, 4 * Time.deltaTime, 0);
                Quaternion lookRotation = Quaternion.LookRotation(machine.lookRotation);
                machine.transform.rotation = Quaternion.Slerp(machine.transform.rotation, lookRotation, 5 * Time.deltaTime);
                machine.ChangeState(new AttackState(machine));
            }
            else
            {
                machine.ChangeState(new ChaseState(machine));
            }
        }
    }
}
