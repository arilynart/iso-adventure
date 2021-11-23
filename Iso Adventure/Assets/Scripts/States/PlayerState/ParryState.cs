using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class ParryState : PlayerState
    {
        public ParryState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            if (machine.controller.MouseActivityCheck())
            {
                machine.transform.LookAt(machine.controller.GetLookPoint());
            }
            machine.controller.animator.SetTrigger("Parry");
            float time = 0;
            machine.parrying = true;
            while (time < machine.parryDuration)
            {
                //Debug.Log("Parry active.");
                time += Time.deltaTime;
                yield return null;
            }
            machine.parrying = false;
            //Debug.Log("Parry deactivated.");
            while (time < machine.controller.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                yield return null;
            }
            machine.controller.animator.ResetTrigger("Parry");
            machine.ChangeState(new IdleState(machine));
        }
    }
}
