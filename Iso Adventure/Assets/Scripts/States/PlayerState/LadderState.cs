using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class LadderState : PlayerState
    {
        public LadderState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.controller.transform.position = machine.controller.currentLadder.startPosition.position;
            machine.controller.transform.LookAt(machine.controller.currentLadder.LookPoint);
            machine.controller.animator.SetBool("Speed", false);
            machine.controller.animator.SetBool("Falling", false);
            machine.controller.onLadder = true;
            machine.rb.useGravity = false;

            yield break;
        }

        public override void LocalFixedUpdate()
        {
            machine.controller.LadderMove();
        }

        public override void Interact()
        {
            machine.controller.LeaveLadder();
        }
    }
}
