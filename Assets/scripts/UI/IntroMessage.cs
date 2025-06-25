using UnityEngine;
using TMPro;
using System.Collections;

public class IntroMessage : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;
    public float displayTime = 3f;

    void Start()
    {
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        yield return FadeIn();
        yield return new WaitForSeconds(displayTime);
        yield return FadeOut();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
    }
}
