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

    public static void FADEOUT(string scene)
    {
        if (!FADER.black) return;
        FADER.black.DOColor(new Color(0, 0, 0, 1), 1).OnComplete(() => SCENELOAD(scene));
        //
    }

    public static void FADEIN()
    {
        if (!FADER.black) return;
        FADER.StartCoroutine(FADETIME());
    }

    static IEnumerator FADETIME()
    {
        yield return new WaitForSeconds(0.5f);
        FADER.black.DOColor(new Color(0, 0, 0, 0), 1);

    }

    public static void SCENELOAD(string scene)
    {
        SceneManager.LoadScene(scene);
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FADEIN();
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
