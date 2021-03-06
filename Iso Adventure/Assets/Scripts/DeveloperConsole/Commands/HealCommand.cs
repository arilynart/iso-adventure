using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.DeveloperConsole.Commands
{
    [CreateAssetMenu(fileName = "New HealCommand", menuName = "DeveloperConsole/HealCommand")]
    public class HealCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                Debug.Log("Invalid number of arguments.");
                return false;
            }
            int i = 0;
            foreach (string argument in args)
            {  
                if (argument == string.Empty)
                {
                    Debug.Log("Missing argument at index " + i);
                    return false;
                }
                i++;
            }
            if (!int.TryParse(args[0], out int value))
            {
                Debug.Log("Could not parse value as int.");
                return false;
            }

            Debug.Log("Healing Damage: " + value);
            DeveloperConsoleBehavior.PLAYER.GetComponent<PlayerHealth>().HealDamage(value);

            return true;
        }
    }

}