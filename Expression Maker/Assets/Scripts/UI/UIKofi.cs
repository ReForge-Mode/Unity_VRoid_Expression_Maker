using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKofi : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image imageBackground;
    public Image imageText;
    public float fadeDuration = 1.0f;
    public float fadeOutDelay = 6.0f;
    public int index = 0;

    private bool isRunning = false;
    private Coroutine coroutine;

    public List<Promotion> promotionList;

    public void StartPromotion()
    {
        if(isRunning == true)
        {
            StopCoroutine(coroutine);
        }

        StartCoroutine(FadeInCoroutine());
        coroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        isRunning = true;

        //Increment
        index++;
        index %= promotionList.Count;

        rectTransform.sizeDelta = new Vector2(promotionList[index].width, rectTransform.sizeDelta.y);
        imageBackground.color = promotionList[index].colorBg;
        imageText.sprite = promotionList[index].spriteText;

        // Fade in the image
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime / fadeDuration;
            imageBackground.color = new Color(imageBackground.color.r, imageBackground.color.g, imageBackground.color.b, a);
            imageText.color = new Color(imageText.color.r, imageText.color.g, imageText.color.b, a);
            yield return null;
        }
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
            imageBackground.color = new Color(imageBackground.color.r, imageBackground.color.g, imageBackground.color.b, a);
            imageText.color = new Color(imageText.color.r, imageText.color.g, imageText.color.b, a);
            yield return null;
        }

        isRunning = false;
    }

    public void OpenLink()
    {
        Application.OpenURL(promotionList[index].link);
    }

    [System.Serializable]
    public struct Promotion
    {
        public string link;
        public Sprite spriteText;
        public Color colorBg;
        public float width;
    }
}
