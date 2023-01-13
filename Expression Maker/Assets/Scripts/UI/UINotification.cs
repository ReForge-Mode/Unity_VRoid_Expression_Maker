using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI tmp;
    public float fadeDuration = 1.0f;
    public float fadeOutDelay = 6.0f;
    public bool fadeIn = true;

    private bool isRunning = false;
    private Coroutine coroutine;

    private void StartFadeOut()
    {
        if(isRunning == true)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(FadeOutCoroutine());
    }

    public void StartFadeIn(string message)
    {
        tmp.text = message;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.75f);
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1);

        StartFadeOut();
    }

    private IEnumerator FadeOutCoroutine()
    {
        isRunning = true;

        // Wait for the specified delay
        yield return new WaitForSeconds(fadeOutDelay);

        // Fade the image
        float a = 1;
        while (a > 0)
        {
            a -= Time.deltaTime / fadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a/2);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, a);
            yield return null;
        }

        isRunning = false;
    }
}
