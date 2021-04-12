using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private CanvasGroup canvasgrp;

    private void Awake()
    {
        canvasgrp = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(this);
    }

    public IEnumerator FadeOutIn(float fadeTime)
    {
        yield return FadeOut(fadeTime);
        yield return FadeIn(fadeTime);
    }
    public IEnumerator FadeOut(float fadeTime)
    {
        while(canvasgrp.alpha < 1)
        {
            canvasgrp.alpha += Time.deltaTime / fadeTime;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float fadeTime)
    {
        while(canvasgrp.alpha > 0)
        {
            canvasgrp.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}
