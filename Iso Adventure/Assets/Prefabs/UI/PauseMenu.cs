using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;

    public GameObject pauseFirstButton;

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

            EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            //clear selected object
            es.SetSelectedGameObject(null);

            es.SetSelectedGameObject(INSTANCE.pauseFirstButton);
        }
        else
        {
            INSTANCE.pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public static void QUIT()
    {
        Application.Quit();
    }

}
