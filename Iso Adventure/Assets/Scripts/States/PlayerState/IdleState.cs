using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState {
    public class IdleState : PlayerState
    {
        public IdleState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.controller.animator.SetBool("Speed", false);
            machine.controller.animator.SetBool("Falling", false);
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.controller.grounded)
            {
                machine.ChangeState(new FallState(machine));
            }
        }

        public override void Movement(Vector2 value)
        {
            if (value != Vector2.zero)
                machine.ChangeState(new WalkState(machine));
        }

        public override void Dodge()
        {
            if(!machine.dashDelay) machine.ChangeState(new DodgeState(machine));
        }

        public override void BasicAttack()
        {
            machine.ChangeState(new BasicAttackState(machine));
        }

        public override void Shoot()
        {
            machine.ChangeState(new ShootState(machine));
        }

        public override void Heal()
        {
            machine.ChangeState(new HealState(machine));
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
