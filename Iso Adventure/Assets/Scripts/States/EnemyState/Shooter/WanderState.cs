using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Shooter
{
    public class WanderState : EnemyState
    {
        public WanderState(IEnemyStateMachine mch) : base(mch) { }

        bool wanderTrigger = false;

        public override IEnumerator EnterState()
        {
            machine.Agent.speed = 0.8f;
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.Agent.enabled) return;
            if (!machine.Agent.pathPending && machine.Agent.remainingDistance <= 0.1f && !wanderTrigger)
            {

                wanderTrigger = true;
                machine.Controller.StartCoroutine(WanderDelay(Random.Range(2, 4)));

            }
            if (machine.CanSeePlayer)
            {
                //change state to chase
                machine.Controller.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
            }
        }

        IEnumerator WanderDelay(float duration)
        {
            machine.Animator.SetBool("Walking", false);
            Debug.Log("Wandering");
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

            if (NavMesh.SamplePosition(targetPosition, out hit, 20, machine.Agent.areaMask))
            {
                if (machine.Agent.enabled)
                {
                    machine.Agent.SetDestination(hit.position);
                    machine.Animator.SetBool("Walking", true);
                }
            }
            wanderTrigger = false;
        }
    }
}
