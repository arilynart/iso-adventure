using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.EnemyState.Shooter
{
    public class DeadState : EnemyState
    {
        public DeadState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.Agent.enabled = false;
            yield break;
        }
    }
}
