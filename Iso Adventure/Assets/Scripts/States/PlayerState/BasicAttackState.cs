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
        float time = 0f;

        public override IEnumerator EnterState()
        {
            if (machine.controller.MouseActivityCheck())
            {
                machine.transform.LookAt(machine.controller.GetLookPoint());
            }

            //animation controls go here
            machine.lastPressedTime = System.DateTime.Now;
            switch (machine.noOfPresses) {
                case 0:
                    
                    machine.StartCoroutine(PlayAnimation("BasicAttackTrigger"));
                    //Debug.Log("You attack once!");
                    break;
                case 1:
                    machine.StartCoroutine(PlayAnimation("BasicAttackTrigger2"));
                    //Debug.Log("You attack twice!");
                    break;
                case 2:
                    machine.StartCoroutine(PlayAnimation("BasicAttackTrigger3"));
                    //Debug.Log("You attack thrice!");
                    break;
            }

            machine.noOfPresses++;
            yield break;
        }

        public override void LocalFixedUpdate()
        {
            if (time > 0.25f) return;
            machine.rb.velocity += machine.controller.point * machine.attackMoveSpeed * 2;
        }

        public override void BasicAttack()
        {
            if (buffer)
            {
                toggle = true;
            }
        }

        IEnumerator PlayAnimation(string trigger)
        {
            time = 0;
            machine.controller.animator.SetTrigger(trigger);
            machine.slash.SetActive(true);
            machine.slashAnim.SetTrigger("slash");
            machine.sword.SetActive(true);
            while (time < 0.1667f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            buffer = true;
            Collider[] colliders = Physics.OverlapSphere(machine.attackPoint.position, 0.99f);
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
                    if (Physics.Raycast(machine.transform.position + raycastOffset, machine.transform.forward + raycastOffset, out hit, 10f, machine.controller.ground))
                    {

                        Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
                        Vector3 localDir = localPoint.normalized;

                        push.Slide(localDir);
                        break;
                    }

                }
                else if (breakable)
                {
                    breakable.Hit();
                }
                else if (reset)
                {
                    reset.Restart();
                    break;
                }
            }

            while (time < machine.controller.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                //Debug.Log("AnimationTime: " + time);
                yield return null;
            }
            machine.rb.velocity = Vector3.zero;
            //yield return null;
            buffer = false;
            machine.controller.animator.ResetTrigger(trigger);
            machine.slash.SetActive(false);
            machine.slashAnim.ResetTrigger("slash");
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
