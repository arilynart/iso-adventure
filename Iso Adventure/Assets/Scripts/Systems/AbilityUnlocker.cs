using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.AbilityUnlock
{
    public class AbilityUnlocker : MonoBehaviour
    {
        public enum Ability
        {
            Attack,
            Blink,
            Fireball
        };
        public Ability ability;
        public void UnlockAbility()
        {
            switch (ability)
            {
                case Ability.Attack:
                    PlayerUnlocks.BASIC_ATTACK = true;
                    break;
                case Ability.Blink:
                    PlayerUnlocks.BLINK = true;
                    break;
                case Ability.Fireball:
                    PlayerUnlocks.FIREBALL = true;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerController>()) return;

            UnlockAbility();
            Destroy(gameObject);
        }
    }
}
