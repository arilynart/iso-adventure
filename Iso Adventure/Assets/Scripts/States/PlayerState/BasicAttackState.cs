using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class BasicAttackState : PlayerState
    {
        public BasicAttackState(PlayerStateMachine mch): base(mch) { }

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
                    machine.controller.animator.SetTrigger("BasicAttackTrigger");
                    machine.StartCoroutine(PlayAnimation(0.13f, 0.32f));
                    machine.noOfPresses = 1;
                    Debug.Log("You attack once!");
                    break;
                case 1:
                    machine.controller.animator.SetTrigger("BasicAttackTrigger2");
                    machine.StartCoroutine(PlayAnimation(0.12f, 0.25f));
                    machine.noOfPresses = 2;
                    Debug.Log("You attack twice!");
                    break;
                case 2:
                    machine.controller.animator.SetTrigger("BasicAttackTrigger3");
                    machine.StartCoroutine(PlayAnimation(0.12f, 0.25f));
                    machine.noOfPresses = 3;
                    Debug.Log("You attack thrice!");
                    break;
            }
            yield break;
        }

        IEnumerator PlayAnimation(float hurtBoxStart, float hurtBoxEnd)
        {
            machine.sword.SetActive(true);
            float time = 0;
            while (time < machine.controller.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                if (hurtBoxStart >= 0 || hurtBoxEnd >= 0)
                {
                    if (time >= hurtBoxStart && time < hurtBoxEnd) activateHurtbox();
                    if (time > hurtBoxEnd) deactivateHurtbox();
                }

                time += Time.deltaTime;
                yield return null;
            }
            machine.sword.SetActive(false);
            machine.controller.animator.ResetTrigger("BasicAttackTrigger");
            machine.controller.animator.ResetTrigger("BasicAttackTrigger2");
            machine.controller.animator.ResetTrigger("BasicAttackTrigger3");
            machine.ChangeState(new IdleState(machine));
        }

        public void activateHurtbox()
        {
            machine.attackCollider.enabled = true;
        }

        public void deactivateHurtbox()
        {
            machine.attackCollider.enabled = false;
        }
    }
}
