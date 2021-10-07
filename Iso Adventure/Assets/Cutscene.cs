using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using Bolt;
using Arilyn.DeveloperConsole.Behavior;

public class Cutscene : MonoBehaviour
{
    CameraFollow follow;
    void Start()
    {
        follow = GetComponent<CameraFollow>();
    }

    public void CutsceneStart(Transform target, float duration)
    {
        StartCoroutine(CutscenePlay(target, duration));
    }

    IEnumerator CutscenePlay(Transform target, float duration)
    {
        follow.lerpTime = 0.04f;
        Transform oldPos = follow.target;
        Variables.Object(DeveloperConsoleBehavior.PLAYER.gameObject).Set("animLock", true);
        float time = 0;
        while (time < duration)
        {
            follow.target = target;
            time += Time.deltaTime;
            yield return null;
        }
        follow.target = oldPos;
        follow.lerpTime = 0.1f;
        Variables.Object(DeveloperConsoleBehavior.PLAYER.gameObject).Set("animLock", false);
    }
}
