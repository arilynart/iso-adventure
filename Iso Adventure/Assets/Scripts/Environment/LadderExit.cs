using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class LadderExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        DeveloperConsoleBehavior.PLAYER.LeaveLadder();
    }
}
