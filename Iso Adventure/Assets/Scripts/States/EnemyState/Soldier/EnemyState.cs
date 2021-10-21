using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.EnemyState
{
    public abstract class EnemyState : State
    {
        public EnemyStateMachine machine;

        public EnemyState(EnemyStateMachine mch)
        {
            machine = mch;
        }
    }
}
