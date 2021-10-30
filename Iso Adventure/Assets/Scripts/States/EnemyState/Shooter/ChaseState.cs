using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Shooter
{
    public class ChaseState : EnemyState
    {
        public ChaseState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.Animator.SetBool("Walking", true);
            machine.Controller.Attack();
            yield break;
        }

        public override void LocalUpdate()
        {
            if (machine.Agent.enabled)
            {
                machine.Agent.SetDestination(DeveloperConsoleBehavior.PLAYER.transform.position);
                /*                Vector3 targetRot = Vector3.RotateTowards(machine.transform.forward, machine.lookRotation, 4 * Time.deltaTime, 0);
                                Quaternion lookRotation = Quaternion.LookRotation(targetRot);
                                machine.transform.rotation = lookRotation;*/
                Quaternion lookRotation = Quaternion.LookRotation(machine.LookRotation);
                machine.Transform.rotation = Quaternion.Slerp(machine.Transform.rotation, lookRotation, 5 * Time.deltaTime);


            }
            if (!machine.CanSeePlayer)
            {
                machine.Controller.StartCoroutine(machine.ChangeState(new WanderState(machine)));
            }
            else if (machine.AttackDistance < machine.Stats.range)
            {
                machine.Controller.StartCoroutine(machine.ChangeState(new AimState(machine)));
            }
        }
    }
}
