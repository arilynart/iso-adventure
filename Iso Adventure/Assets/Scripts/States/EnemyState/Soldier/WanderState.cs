using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Arilyn.State.EnemyState.Soldier
{
    public class WanderState : EnemyState
    {
        public WanderState(EnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.agent.speed = 0.8f;
            AnimatorStateInfo info = machine.animator.GetCurrentAnimatorStateInfo(0);
            float normal = info.normalizedTime;
            if (normal != 0)
            {
                machine.animator.Play("ZombieIdle");
            }
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.agent.pathPending && machine.agent.remainingDistance <= 2)
            {
                machine.StartCoroutine(WanderDelay(Random.Range(2, 4)));
            }
            if (machine.canSeePlayer)
            {
                //change state to chase
                machine.ChangeState(new ChaseState(machine));
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
            targetPosition += machine.transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(targetPosition, out hit, 5, machine.agent.areaMask)) {
                machine.agent.SetDestination(hit.position);
            }
        }
    }
}
