using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.DeveloperConsole.Commands
{
    [CreateAssetMenu(fileName = "New UnlockCommand", menuName = "DeveloperConsole/UnlockCommand")]
    public class UnlockCommand : ConsoleCommand
    {

        public override bool Process(string[] args)
        {
            if (args.Length != 1) return false;
            foreach (string argument in args)
            {
                if (argument == string.Empty) return false;
            }

            if (args[0] == "attack")
            {
                PlayerUnlocks.BASIC_ATTACK = !PlayerUnlocks.BASIC_ATTACK;
                Debug.Log("Attack: " + PlayerUnlocks.BASIC_ATTACK);
            }
            else if (args[0] == "blink")
            {
                PlayerUnlocks.BLINK = !PlayerUnlocks.BLINK;
                Debug.Log("Blink: " + PlayerUnlocks.BLINK);
            }
            else if (args[0] == "fireball")
            {
                PlayerUnlocks.FIREBALL = !PlayerUnlocks.FIREBALL;
                Debug.Log("Fireball: " + PlayerUnlocks.FIREBALL);
            }
            else if (args[0] == "all")
            {
                PlayerUnlocks.BASIC_ATTACK = !PlayerUnlocks.BASIC_ATTACK;
                PlayerUnlocks.BLINK = !PlayerUnlocks.BLINK;
                PlayerUnlocks.FIREBALL = !PlayerUnlocks.FIREBALL;
            }
            else return false;

            return true;
        }
    }
}
