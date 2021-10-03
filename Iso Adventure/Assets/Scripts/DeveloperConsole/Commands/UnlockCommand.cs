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
                PlayerUnlocks.ATTACK = !PlayerUnlocks.ATTACK;
                Debug.Log("Attack: " + PlayerUnlocks.ATTACK);
            }
            if (args[0] == "blink")
            {
                PlayerUnlocks.BLINK = !PlayerUnlocks.BLINK;
                Debug.Log("Blink: " + PlayerUnlocks.BLINK);
            }
            if (args[0] == "fireball")
            {
                PlayerUnlocks.FIREBALL = !PlayerUnlocks.FIREBALL;
                Debug.Log("Fireball: " + PlayerUnlocks.FIREBALL);
            }
            else return false;

            return true;
        }
    }
}
