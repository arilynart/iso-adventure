using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public IEnumerator ActivateDissolve(float v_start, float v_end, float duration, float delay)
    {
        float elapsed = 0;

        while (elapsed < delay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        float timer = 0f;
        while (timer < duration)
        {
            material.SetFloat("DissolveAmount", (Mathf.Lerp(v_start, v_end, timer / duration)));
            timer += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("DissolveAmount", v_end);
    }
}
