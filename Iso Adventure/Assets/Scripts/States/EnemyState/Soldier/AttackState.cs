using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


namespace Arilyn.State.EnemyState.Soldier
{
    public class AttackState : EnemyState
    {
        public AttackState(IEnemyStateMachine mch) : base(mch) { }


        public override IEnumerator EnterState()
        {
            machine.Animator.SetTrigger(machine.Stats.animationName);
            DeveloperConsoleBehavior.PLAYER.StartCoroutine(StopDelay(machine.Stats.boxStart, machine.Stats.boxEnd));
            yield break;
        }

        public override IEnumerator ExitState()
        {
            machine.Agent.acceleration = machine.Acceleration;
            machine.Agent.speed = machine.Speed;
            machine.Animator.ResetTrigger(machine.Stats.animationName);
            machine.Stats.NextAttack();
            yield break;
        }

        IEnumerator StopDelay(float hurtBoxStart, float hurtBoxEnd)
        {
            if (machine.Agent.enabled) machine.Agent.SetDestination(machine.Transform.position);
            machine.Agent.acceleration = 0;
            machine.Agent.speed = 0;
            float time = 0;
            while (time < machine.Animator.GetCurrentAnimatorStateInfo(0).length)
            {
                if (time < hurtBoxStart)
                {
                    //turn towards player
                    Quaternion lookRotation = Quaternion.LookRotation(machine.LookRotation);
                    machine.Transform.rotation = Quaternion.Slerp(machine.Transform.rotation, lookRotation, 5 * Time.deltaTime);
                }
                else if (time >= hurtBoxStart && time <= hurtBoxEnd)
                {
                    //stop and hit
                    Collider[] hits = Physics.OverlapBox(machine.Controller.hurtBox.transform.position, new Vector3(0.36f, 0.415f, 1.01f));

                    foreach (Collider hit in hits)
                    {
                        PlayerHealth health = hit.GetComponent<PlayerHealth>();
                        PlayerController controller = hit.GetComponent<PlayerController>();
                        if (health && controller)
                        {
                            if (!controller.invuln) health.TakeDamage(machine.Controller.stats.damage);
                        }
                    }
                }


                time += Time.deltaTime;
                yield return null;
            }

            DeveloperConsoleBehavior.PLAYER.StartCoroutine(machine.ChangeState(new ChaseState(machine)));

        }
    }
}
