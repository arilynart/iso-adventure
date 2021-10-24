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

        bool trigger = false;
        bool wanderTrigger = false;

        public override IEnumerator EnterState()
        {
            machine.Agent.speed = 0.8f;
/*            AnimatorStateInfo info = machine.Animator.GetCurrentAnimatorStateInfo(0);
            float normal = info.normalizedTime;
            if (normal != 0)
            {
                machine.Animator.Play("ZombieIdle");
            }*/
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.Agent.enabled) return;
            if (!machine.Agent.pathPending && machine.Agent.remainingDistance <= 0.1f && !wanderTrigger)
            {

                wanderTrigger = true;
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(WanderDelay(Random.Range(2, 4)));
                
            }
            if (machine.CanSeePlayer && !trigger)
            {
                //change state to chase
                DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
                trigger = true;
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
            machine.Animator.SetBool("Walking", true);
            //find position to wander
            Vector3 targetPosition = (new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * 5);
            targetPosition += machine.Transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(targetPosition, out hit, 5, machine.Agent.areaMask)) {
                if (machine.Agent.enabled) machine.Agent.SetDestination(hit.position);
            }
            wanderTrigger = false;
        }
    }
}
