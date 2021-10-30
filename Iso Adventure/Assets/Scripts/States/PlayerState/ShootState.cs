using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class ShootState : PlayerState
    {
        public ShootState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            Debug.Log("Shooting");

            machine.controller.animator.SetTrigger("ShootTrigger");
            machine.controller.mana.AddMana(-machine.fireballCost);

            machine.CreateFireball();
            //enter fireball animation
            machine.StartCoroutine(PlayAnimation());
            yield break;
        }
        IEnumerator PlayAnimation()
        {
            float time = 0;
            while (time < machine.controller.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                time += Time.deltaTime;
                yield return null;
            }
            machine.controller.animator.ResetTrigger("ShootTrigger");
            machine.ChangeState(new IdleState(machine));
        }
    }
}