using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.DeveloperConsole.Commands
{
    [CreateAssetMenu(fileName = "New TeleportCommand", menuName = "DeveloperConsole/TeleportCommand")]
    public class TeleportCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            if (args.Length != 3) return false;
            foreach (string argument in args)
            {
                if (argument == string.Empty) return false;
            }
            if (!int.TryParse(args[0], out int value1)) return false;
            if (!int.TryParse(args[1], out int value2)) return false;
            if (!int.TryParse(args[2], out int value3)) return false;

            DeveloperConsoleBehavior.PLAYER.transform.position = new Vector3(value1, value2, value3);

            return true;
        }
    }

}