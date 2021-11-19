using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public class ParryState : PlayerState
    {
        public ParryState(PlayerStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            yield break;
        }
    }
}
