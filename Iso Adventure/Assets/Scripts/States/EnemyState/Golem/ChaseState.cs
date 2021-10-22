using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Golem
{
    public class ChaseState : EnemyState
    {
        public ChaseState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.Animator.SetBool("Walking", true);
            machine.Controller.Attack();
            if (machine.Stats.activeAttack == machine.Stats.lockedAttacks[2])
            {
                Vector3 point = DeveloperConsoleBehavior.PLAYER.GetComponent<Collider>().ClosestPoint(machine.Transform.position);
                machine.Transform.position = point;
            }
            yield return null;
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
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new WanderState(machine)));
            }
            else if (machine.AttackDistance < machine.Stats.range)
            {
                if (machine.Stats.activeAttack == machine.Stats.attacks[2] || machine.Stats.activeAttack == machine.Stats.lockedAttacks[2])
                {
                    machine.Transform.LookAt(DeveloperConsoleBehavior.PLAYER.transform);

                    
                }
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new AimState(machine)));
            }
        }
    }
}
