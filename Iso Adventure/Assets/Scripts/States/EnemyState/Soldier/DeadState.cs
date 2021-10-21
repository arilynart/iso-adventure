using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.EnemyState.Soldier
{
    public class DeadState : EnemyState
    {
        public DeadState(EnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.agent.enabled = false;
            yield break;
        }
    }
}
