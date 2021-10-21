using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State
{
    public abstract class State
    {


        public virtual IEnumerator EnterState()
        {
            yield break;
        }

        public virtual IEnumerator ExitState()
        {
            yield break;
        }

        public virtual void LocalUpdate()
        {

        }
    }
}
