using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderAreaExpand : MonoBehaviour
{
    public float heightPerItem;
    public RectTransform content;

    public void UpdateSliderAreaHeight(int itemCount)
    {
        float heightTotal = itemCount * heightPerItem;
        content.sizeDelta = new Vector2(content.sizeDelta.x, heightTotal);
    }
}
