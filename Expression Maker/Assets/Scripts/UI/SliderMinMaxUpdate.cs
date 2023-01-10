using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates all sliders in the scene min max value to match the field at the top
/// </summary>
public class SliderMinMaxUpdate : MonoBehaviour
{
    private Slider[] sliderList;

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
            //float value = slider.value;
            slider.maxValue = float.Parse(inputField.text);
            //slider.value = value;
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
