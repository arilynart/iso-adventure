using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class WalkState : PlayerState
    {
        public WalkState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.controller.animator.SetBool("Speed", true);
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.controller.grounded)
            {
                machine.ChangeState(new FallState(machine));
            }
        }

        public override void LocalFixedUpdate()
        {
            machine.controller.Move();
        }

        public override void Movement(Vector2 value)
        {
            if (value == Vector2.zero)
            {
                machine.controller.animator.SetBool("Speed", false);
                machine.ChangeState(new IdleState(machine));
            }
        }

        public override void Dodge()
        {
            if (!machine.dashDelay)
            {
                machine.controller.animator.SetBool("Speed", false);
                machine.ChangeState(new DodgeState(machine));
            }
        }

        public override void BasicAttack()
        {
            machine.controller.animator.SetBool("Speed", false);
            machine.ChangeState(new BasicAttackState(machine));
        }

        public override void Shoot()
        {
            machine.controller.animator.SetBool("Speed", false);
            machine.ChangeState(new ShootState(machine));
        }

        public override void Heal()
        {
            machine.controller.animator.SetBool("Speed", false);
            machine.ChangeState(new HealState(machine));
        }

        public override void Blink()
        {
            machine.controller.animator.SetBool("Speed", false);
            machine.controller.blink.Blink();
        }

        public override void Interact()
        {
            machine.controller.interactTrigger = true;
        }
    }
}
