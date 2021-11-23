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
            machine.controller.animator.SetLayerWeight(1, 1);
            yield break;
        }

        public override void LocalUpdate()
        {
            if (!machine.controller.grounded)
            {
                ResetAnimation();
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
                ResetAnimation();   
                machine.ChangeState(new IdleState(machine));
            }
        }

        public override void Dodge()
        {
            if (!machine.dashDelay)
            {
                ResetAnimation();
                machine.ChangeState(new DodgeState(machine));
            }
        }

        public override void BasicAttack()
        {
            ResetAnimation();
            machine.ChangeState(new BasicAttackState(machine));
        }

        public override void Shoot()
        {
            ResetAnimation();
            machine.ChangeState(new ShootState(machine));
        }

        public override void Heal()
        {
            ResetAnimation();
            machine.ChangeState(new HealState(machine));
        }

        public override void Blink()
        {
            ResetAnimation();
            machine.controller.blink.Blink();
        }

        public override void Parry()
        {
            ResetAnimation();
            machine.ChangeState(new ParryState(machine));
        }

        public override void Interact()
        {
            machine.controller.interactTrigger = true;
        }

        void ResetAnimation()
        {
            machine.controller.animator.SetLayerWeight(1, 0);
            machine.controller.animator.SetBool("Speed", false);
        }
    }
}
