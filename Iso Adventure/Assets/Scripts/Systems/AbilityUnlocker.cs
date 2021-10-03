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
            Blink
        };
        public Ability ability;
        public void UnlockAbility()
        {
            switch (ability)
            {
                case Ability.Attack:
                    PlayerUnlocks.ATTACK = true;
                    break;
                case Ability.Blink:
                    PlayerUnlocks.BLINK = true;
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
