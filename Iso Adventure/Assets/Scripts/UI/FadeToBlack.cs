using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeToBlack : MonoBehaviour
{
    public Image black;
    public Color color;

    public static FadeToBlack FADER;
    static bool FADING;
    public static bool FADEAWAY;

    public float initialFade = 2;
      
    private void Awake()
    {
        if (FADER != null && FADER != this)
        {
            Destroy(gameObject);
            return;
        }

        FADER = this;

        
    }

    private void Update()
    {
        //black.color = color;
    }

    public static void FADEOUT()
    {
        if (!FADER.black || FADING) return;

        FADING = true;
        FADEAWAY = true;
        FADER.black.DOColor(new Color(0, 0, 0, 1), 0.5f).OnComplete(() => FADEIN(0.5f, 0.2f));
    }

    public static void FADEOUT(string scene)
    {
        if (!FADER.black || FADING) return;

        FADING = true;
        FADER.black.DOColor(new Color(0, 0, 0, 1), 1).OnComplete(() => SCENELOAD(scene));
        //
    }

    public static void FADEIN(float fadeTime, float fadeDelay)
    {
        if (!FADER.black) return;

        FADEAWAY = false;
        FADER.StartCoroutine(FADETIME(fadeTime, fadeDelay));
    }

    static IEnumerator FADETIME(float fadeTime, float fadeDelay)
    {
        yield return new WaitForSeconds(fadeDelay);
        FADER.black.DOColor(new Color(0, 0, 0, 0), fadeTime).OnComplete(() => FADING = false);

    }

    public static void SCENELOAD(string scene)
    {
        SceneManager.LoadScene(scene);
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FADEIN(1, initialFade);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
