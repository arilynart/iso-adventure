using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class DodgeState : PlayerState
    {
        public DodgeState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.controller.airDodge = true;
            if (machine.controller.groundAngle != 90)
                machine.rb.AddForce(0, -machine.controller.slopeForce * 50, 0);

            if (machine.controller.move != Vector2.zero)
            {
                //snap player rotation to inputted direction
                machine.controller.point = machine.controller.headPoint;
                machine.transform.forward = machine.controller.head;
            }
            //Start movement
            machine.controller.animator.SetTrigger("DodgeTrigger");
            machine.StartCoroutine(DodgeMovement(machine.dashDuration));
            yield break;
        }

        IEnumerator DodgeMovement(float duration)
        {
            Debug.Log("Moving Dodge");
            //reset timer
            float time = 0f;
            machine.StartCoroutine(machine.controller.health.Invulnerability(machine.dashTime));
            Physics.IgnoreLayerCollision(3, 7, true);
            Physics.IgnoreLayerCollision(3, 11, true);

            while (time < machine.dashTime)
            {

                //for the first 0.3s of the dodge
                machine.rb.velocity = machine.controller.point * machine.dashSpeed;

                //Increase the timer
                time += Time.deltaTime;

                yield return null;
            }
            Physics.IgnoreLayerCollision(3, 7, false);
            Physics.IgnoreLayerCollision(3, 11, false);

            machine.rb.velocity = Vector3.zero;
            machine.controller.animator.ResetTrigger("DodgeTrigger");
            machine.dashDelay = true;
            machine.ChangeState(new IdleState(machine));

            yield return new WaitForSeconds(duration - machine.dashTime);

            machine.dashDelay = false;
        }
    }
}
