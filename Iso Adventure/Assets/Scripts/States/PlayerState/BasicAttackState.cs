using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class BasicAttackState : PlayerState
    {
        public BasicAttackState(PlayerStateMachine mch): base(mch) { }
        Vector3 raycastOffset = new Vector3(0, 0.5f, 0);

        bool buffer = false;
        bool toggle = false;

        public override IEnumerator EnterState()
        {
            if (machine.controller.MouseActivityCheck())
            {
                machine.transform.LookAt(machine.controller.GetLookPoint());
            }
            //detect enemies in range of attack


            //animation controls go here
            machine.lastPressedTime = System.DateTime.Now;
            switch (machine.noOfPresses) {
                case 0:
                    
                    machine.StartCoroutine(PlayAnimation(0.13f, 0.32f, "BasicAttackTrigger"));
                    //Debug.Log("You attack once!");
                    break;
                case 1:
                    machine.StartCoroutine(PlayAnimation(0.12f, 0.25f, "BasicAttackTrigger2"));
                    //Debug.Log("You attack twice!");
                    break;
                case 2:
                    machine.StartCoroutine(PlayAnimation(0.12f, 0.25f, "BasicAttackTrigger3"));
                    //Debug.Log("You attack thrice!");
                    break;
            }

            machine.noOfPresses++;
            yield break;
        }

        public override void BasicAttack()
        {
            if (buffer)
            {
                toggle = true;
            }
        }

        IEnumerator PlayAnimation(float hurtBoxStart, float hurtBoxEnd, string trigger)
        {
            machine.controller.animator.SetTrigger(trigger);
            machine.sword.SetActive(true);
            float time = 0;
            Collider[] colliders = Physics.OverlapSphere(machine.attackPoint.position, 0.66f);
            foreach (Collider col in colliders)
            {
                EnemyStats stats = col.GetComponent<EnemyStats>();
                BlockPush push = col.GetComponent<BlockPush>();
                Breakable breakable = col.GetComponent<Breakable>();
                BlockReset reset = col.GetComponent<BlockReset>();
                if (stats)
                {

                    //calling damage method on collided enemy
                    stats.TakeDamage(machine.attackDamage);
                    //mana restoration
                    machine.controller.mana.AddMana(1);
                }
                else if (push)
                {
                    RaycastHit hit;
                    Debug.Log("Hit Block");
                    if (Physics.Raycast(machine.transform.position + raycastOffset, machine.transform.parent.forward + raycastOffset, out hit, 5f, machine.controller.ground))
                    {

                        Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
                        Vector3 localDir = localPoint.normalized;

                        push.Slide(localDir);
                    }

                }
                else if (breakable)
                {
                    breakable.Hit();
                }
                else if (reset)
                {
                    reset.Restart();
                }
            }
            while (time < machine.controller.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                if (time > machine.controller.animator.GetCurrentAnimatorStateInfo(0).length * 0.25f)
                {
                    buffer = true;
                }

                time += Time.deltaTime;
                //Debug.Log("AnimationTime: " + time);
                yield return null;
            }
            yield return null;
            buffer = false;
            machine.controller.animator.ResetTrigger(trigger);
            if (toggle)
            {
                machine.ChangeState(new BasicAttackState(machine));
            }
            else
            {
                machine.ChangeState(new IdleState(machine));
                machine.sword.SetActive(false);
            }
        }
    }
}
