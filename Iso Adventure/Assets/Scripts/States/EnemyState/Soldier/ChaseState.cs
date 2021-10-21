using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Soldier
{
    public class ChaseState : EnemyState
    {
        public ChaseState(EnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.agent.speed = 1.1f;
            machine.animator.SetBool("Walking", true);
            machine.controller.Attack();
            yield break;
        }

        public override void LocalUpdate()
        {
            if (machine.agent.enabled)
            {
                machine.agent.SetDestination(DeveloperConsoleBehavior.PLAYER.transform.position);
                /*                Vector3 targetRot = Vector3.RotateTowards(machine.transform.forward, machine.lookRotation, 4 * Time.deltaTime, 0);
                                Quaternion lookRotation = Quaternion.LookRotation(targetRot);
                                machine.transform.rotation = lookRotation;*/
                Quaternion lookRotation = Quaternion.LookRotation(machine.lookRotation);
                machine.transform.rotation = Quaternion.Slerp(machine.transform.rotation, lookRotation, 5 * Time.deltaTime);


            }
            if (!machine.canSeePlayer)
            {
                machine.ChangeState(new WanderState(machine));
            }
            else if (machine.attackDistance <  machine.stats.range)
            {
                machine.ChangeState(new AimState(machine));
            }
        }
    }
}
