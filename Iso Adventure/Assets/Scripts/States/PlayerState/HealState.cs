using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class HealState : PlayerState
    {
        public HealState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.controller.animator.SetTrigger("HealTrigger");
            machine.controller.mana.AddMana(-machine.controller.health.healCost);
            machine.StartCoroutine(PlayAnimation());
            yield break;
        }

        IEnumerator PlayAnimation()
        {
            float time = 0;
            while (time < 1.22f)
            {
                if (machine.controller.invuln)
                {
                    machine.controller.animator.Play("Idle_Battle");
                    machine.ChangeState(new IdleState(machine));
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }

            machine.controller.health.HealDamage(machine.controller.health.healValue);

            while (time < machine.controller.animator.GetCurrentAnimatorStateInfo(0).length)
            {
                if (machine.controller.invuln)
                {
                    machine.controller.animator.Play("Idle_Battle");
                    machine.ChangeState(new IdleState(machine));
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }

            machine.controller.animator.ResetTrigger("HealTrigger");
            machine.ChangeState(new IdleState(machine));
        }
    }
}
