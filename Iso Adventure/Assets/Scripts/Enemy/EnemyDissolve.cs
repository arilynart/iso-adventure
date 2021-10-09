using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDissolve : MonoBehaviour
    
{
    public EnemyStats enemyStats;
    public GameObject go;
    public Material material;
    void Start()
    {
        go = this.gameObject;
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStats.hp <= 0)
        {
            StartCoroutine(ActivateDissolve(2f, 0f, 2f, 2));
        }
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
