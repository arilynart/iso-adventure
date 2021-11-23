using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaggerGauge : MonoBehaviour
{
    public static float STAGGER;
    public static GameObject BOSS;
    public static IEnemyStateMachine MACHINE;

    public delegate void StaggerGaugeDelegate(float stagger);
    public static event StaggerGaugeDelegate OnStaggerChanged;

    public Image gauge;

    void OnEnable()
    {
        OnStaggerChanged += UpdateStagger;
    }

    private void OnDisable()
    {
        OnStaggerChanged -= UpdateStagger;
    }

    public static void ADD_STAGGER(float amount)
    {
        STAGGER += amount;
        OnStaggerChanged(STAGGER);
    }

    public static void RESET_STAGGER()
    {
        STAGGER = 0;
        OnStaggerChanged(STAGGER);
    }

    void UpdateStagger(float stagger)
    {
        if (stagger == 0)
        {
            gauge.gameObject.SetActive(false);
            gauge.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gauge.gameObject.SetActive(true);
            gauge.transform.parent.gameObject.SetActive(true);
        }
        gauge.fillAmount = stagger;

    }
}
