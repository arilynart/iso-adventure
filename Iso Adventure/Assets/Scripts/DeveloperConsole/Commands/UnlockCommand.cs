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
            if (args[0] == string.Empty) return false;

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
            else return false;

            return true;
        }
    }
}
