using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniVRM10;
using Unity.VisualScripting;

public class SlidersSets : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Slider slider;
    public TMP_InputField textField;
    public SkinnedMeshRenderer humanoid = null;
    public UndoStackManager undoStackManager;
    public int blendshapeIndex = -1;
    public float lastValue = 0;

    private void Start()
    {
        undoStackManager = GameObject.Find("Undo Stack Manager").GetComponent<UndoStackManager>();

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

    public void SetValue(float value)
    {
        slider.value = value;
        OnSliderValueChanged(value);
    }

    public void ResetValue()
    {
        undoStackManager.LogChanges(this, slider.value, 0);
        SetValue(0);
    }

    public void SetLastValue()
    {
        lastValue = slider.value;
    }

    public void RecordValue()
    {
        undoStackManager.LogChanges(this, lastValue, slider.value);
    }
}
