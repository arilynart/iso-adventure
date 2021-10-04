using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;

    public static PauseMenu INSTANCE;

    private void Awake()
    {
        if (INSTANCE != null && INSTANCE != this)
        {
            Destroy(gameObject);
            return;
        }

        INSTANCE = this;

    }

    public static void PAUSE()
    {
        if (!INSTANCE.pauseCanvas.activeInHierarchy)
        {
            INSTANCE.pauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            INSTANCE.pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
