using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.DeveloperConsole.Commands
{
    [CreateAssetMenu(fileName = "New GodCommand", menuName = "DeveloperConsole/GodCommand")]
    public class GodCommand : ConsoleCommand
    {
        public static int BASEATTACK;
        public static bool GODMODE = false;

        public override bool Process(string[] args)
        {
            if (args.Length != 0)
            {
                Debug.Log("Invalid number of arguments.");
                return false;
            }
            if (!GODMODE)
            {
                BASEATTACK = DeveloperConsoleBehavior.PLAYER.machine.attackDamage;
            }

            GODMODE = !GODMODE;


            if (!GODMODE)
            {
                DeveloperConsoleBehavior.PLAYER.invuln = false;
                DeveloperConsoleBehavior.PLAYER.machine.attackDamage = BASEATTACK;
            }

            return true;
        }
    }

}