using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using Bolt;
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
        defaultMask = Camera.main.cullingMask;
        skyColor = Camera.main.backgroundColor;
    }

    public void CutsceneStart(Transform target, float duration)
    {
        StartCoroutine(CutscenePlay(target, duration));
    }

    IEnumerator CutscenePlay(Transform target, float duration)
    {
        follow.lerpTime = 0.03f;
        Transform oldPos = follow.target;
        DeveloperConsoleBehavior.PLAYER.controls.Disable();
        
        float time = 0;
        while (time < duration)
        {
            follow.target = target;
            time += Time.deltaTime;
            yield return null;
        }
        follow.target = oldPos;
        follow.lerpTime = 0.1f;
        DeveloperConsoleBehavior.PLAYER.controls.Enable();
        Camera.main.cullingMask = defaultMask;
        Camera.main.backgroundColor = skyColor;
    }
}
