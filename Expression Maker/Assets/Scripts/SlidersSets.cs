using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlidersSets : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Slider slider;
    public TMP_InputField textField;
    public SkinnedMeshRenderer humanoid = null;
    public int blendshapeIndex = -1;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        title.text = gameObject.name;
    }

    public void OnSliderValueChanged(float value)
    {
        textField.text = value.ToString("0");
        UpdateBlendshape(value);
    }

    public void OnTextFieldChanged()
    {
        float value;
        if (float.TryParse(textField.text, out value))
        {
            slider.value = value;
            UpdateBlendshape(value);
        }
    }

    public void UpdateBlendshape(float value)
    {
        humanoid.SetBlendShapeWeight(blendshapeIndex, value);
    }
}
