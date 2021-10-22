using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Soldier
{
    public class WanderState : EnemyState
    {
        public WanderState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.Agent.speed = 0.8f;
            AnimatorStateInfo info = machine.Animator.GetCurrentAnimatorStateInfo(0);
            float normal = info.normalizedTime;
            if (normal != 0)
            {
                machine.Animator.Play("ZombieIdle");
            }
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.Agent.pathPending && machine.Agent.remainingDistance <= 2)
            {
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(WanderDelay(Random.Range(2, 4)));
            }
            if (machine.CanSeePlayer)
            {
                //change state to chase
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
            }
        }

        IEnumerator WanderDelay(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            //find position to wander
            Vector3 targetPosition = (new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * 5);
            targetPosition += machine.Transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(targetPosition, out hit, 5, machine.Agent.areaMask)) {
                machine.Agent.SetDestination(hit.position);
            }
        }
    }
}
