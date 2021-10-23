using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.EnemyState
{
    public abstract class EnemyState : State
    {
        public IEnemyStateMachine machine;

        public EnemyState(IEnemyStateMachine mch)
        {
            machine = mch;
        }
    }
}
