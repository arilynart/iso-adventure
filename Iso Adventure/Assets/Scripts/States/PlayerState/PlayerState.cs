using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.State.PlayerState
{
    public abstract class PlayerState : State
    {
        protected readonly PlayerStateMachine machine;

        public PlayerState(PlayerStateMachine mch)
        {
            machine = mch;
        }
        public virtual void LocalFixedUpdate()
        {

        }
        public virtual void Movement(Vector2 value)
        {

        }

        public virtual void Dodge()
        {

        }

        public virtual void BasicAttack()
        {

        }

        public virtual void Shoot()
        {

        }

        public virtual void Heal()
        {

        }

        public virtual void Blink()
        {

        }

        public virtual void Parry()
        {

        }

        public virtual void Interact()
        {

        }
    }
}