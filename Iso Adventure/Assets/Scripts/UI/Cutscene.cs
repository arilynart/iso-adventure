using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class Cutscene : MonoBehaviour
{
    CameraFollow follow;
    public LayerMask indoors;
    public Color skyColor;

    int defaultMask;
    void Start()
    {
        follow = GetComponent<CameraFollow>();
        defaultMask = CameraFollow.MAINCAMERA.cullingMask;
        skyColor = CameraFollow.MAINCAMERA.backgroundColor;
    }

    public void CutsceneStart(Transform target, float duration)
    {
        StartCoroutine(CutscenePlay(target, duration));
    }

    IEnumerator CutscenePlay(Transform target, float duration)
    {
        Transform oldPos = follow.target;
        DeveloperConsoleBehavior.PLAYER.controls.Disable();
        
        float time = 0;
        while (time < duration)
        {
            CameraFollow.LOCK = false;
            follow.target = target;
            time += Time.deltaTime;
            yield return null;
        }
        follow.target = oldPos;
        DeveloperConsoleBehavior.PLAYER.controls.Enable();
        CameraFollow.MAINCAMERA.cullingMask = defaultMask;
        CameraFollow.MAINCAMERA.backgroundColor = skyColor;
    }
}
