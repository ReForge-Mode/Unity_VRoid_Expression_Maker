using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKofi : MonoBehaviour
{
    public string link;
    public Image image;
    public Image image2;
    public float fadeDuration = 1.0f;
    public float fadeOutDelay = 6.0f;
    public bool fadeIn = true;

    public void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(fadeOutDelay);

        // Fade the image
        float a = 1;
        while (a > 0)
        {
            a -= Time.deltaTime / fadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, a);
            yield return null;
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        // Fade in the image
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime / fadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, a);
            yield return null;
        }
    }

    public void OpenLink()
    {
        Application.OpenURL(link);
    }
}
