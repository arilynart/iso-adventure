using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.EnemyState.Golem
{
    public class DeadState : EnemyState
    {
        public DeadState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.Agent.enabled = false;
            machine.Animator.SetTrigger("die");
            yield break;
        }
    }
}
