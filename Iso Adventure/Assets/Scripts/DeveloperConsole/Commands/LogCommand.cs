using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arilyn.DeveloperConsole.Commands
{
    [CreateAssetMenu(fileName = "New LogCommand", menuName = "DeveloperConsole/LogCommand")]
    public class LogCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);
            Debug.Log(logText);

            return true;
        }
    }
}
