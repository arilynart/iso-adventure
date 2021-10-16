using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class FallState : PlayerState
    {
        public FallState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.controller.animator.SetBool("Falling", true);
            yield break;
        }

        public override void LocalUpdate()
        {
            if (machine.controller.grounded)
            {
                machine.controller.animator.SetBool("Falling", false);
                machine.ChangeState(new IdleState(machine));
            }
        }

        public override void LocalFixedUpdate()
        {
            machine.controller.Move();
        }

        public override void Dodge()
        {
            if (!machine.dashDelay) machine.ChangeState(new DodgeState(machine));
        }

        public override void Blink()
        {
            machine.controller.blink.Blink();
        }

        public override void Interact()
        {
            machine.controller.interactTrigger = true;
        }
    }
}
