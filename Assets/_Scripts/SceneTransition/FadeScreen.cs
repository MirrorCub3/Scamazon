using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField]
    bool fadeOnStart = true;
    [SerializeField]
    private float fadeDuration = 1f;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Start()
    {
        if (fadeOnStart) FadeIn();
    }

    public float GetFadeDuration()
    {
        return fadeDuration;
    }
    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {   
            canvasGroup.alpha = Mathf.Lerp(alphaIn, alphaOut, timer/fadeDuration);
            
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = alphaOut;
    }
}
