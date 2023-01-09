using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderMinMaxUpdate : MonoBehaviour
{
    public Slider[] sliderList;

    public void UpdateSliderMin(TMP_InputField inputField)
    {
        foreach (var slider in sliderList)
        {
            slider.minValue = float.Parse(inputField.text);
        }
    }

    public void UpdateSliderMax(TMP_InputField inputField)
    {
        foreach (var slider in sliderList)
        {
            slider.maxValue = float.Parse(inputField.text);
        }
    }

    public void GetSliderList()
    {
        sliderList = null;
        sliderList = FindObjectsOfType<Slider>();
    }

    public enum Option
    {
        Min, Max
    }
}
