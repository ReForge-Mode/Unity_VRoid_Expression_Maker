using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UniVRM10.VRM10Viewer;
using UnityEngine.UI;
using static UniVRM10.VRM10Viewer.VRM10FileDialogForWindows;
using TMPro;
using System.Linq;
using UnityEngine.Events;

public class LoadSliderValues : MonoBehaviour
{
    public SliderCreator sliderCreator;
    public string openFileName;
    public UnityEvent onOpenFileSuccess;
    public BlendshapeData blendshapeData;

    public void ApplyToSliders()
    {
        sliderCreator.SetSlidersValue(0);

        for (int i = 0; i < blendshapeData.root.Count(); i++)
        {
            var name = EditName(blendshapeData.root[i].name);
            Transform slider = sliderCreator.transform.Find(name + " ");
            slider.GetComponent<SlidersSets>().SetValue(blendshapeData.root[i].value);
        }
    }

    public void OpenSingleFile()
    {
        string jsonInput = OpenFile();

        if (!string.IsNullOrEmpty(jsonInput))
        {
            ExtractDataFromJSON(jsonInput);
            ApplyToSliders();
            onOpenFileSuccess.Invoke();
        }
    }

    private string OpenFile()
    {
	string openFilePath;
        #if UNITY_STANDALONE_WIN
        openFilePath = VRM10FileDialogForWindows.FileDialog("Open .fcl file", "fcl");
        #elif UNITY_EDITOR
        openFilePath = UnityEditor.EditorUtility.OpenFilePanel("Open .fcl file", "", "fcl");
        #else
        openFilePath = Application.dataPath + "/default.fcl";
        #endif

        if (!string.IsNullOrEmpty(openFilePath))
        {
            ////Find the name
            var temp = Path.GetFileName(openFilePath);
            openFileName = temp.Substring(0, temp.Length - 4);

            // Use path to load file and parse JSON data
            return System.IO.File.ReadAllText(openFilePath);
        }

        return null;
    }

    private void ExtractDataFromJSON(string jsonInput)
    {
        if (!string.IsNullOrEmpty(jsonInput))
        {
            //Reset array
            blendshapeData.root = null;

            // Parse the JSON data
            BlendshapeData data = JsonUtility.FromJson<BlendshapeData>(jsonInput);
            blendshapeData.root = data.root;
        }
    }

    private string EditName(string input)
    {
        var inputString = input + " ";
        inputString = inputString.Replace("_", " ");
        inputString = inputString.Replace("Fcl ", "");
        inputString = inputString.Replace("ALL", "Full");
        inputString = inputString.Replace("BRW", "Brow");
        inputString = inputString.Replace("MTH", "Mouth");
        inputString = inputString.Replace("EYE", "Eye");
        inputString = inputString.Replace("HA ", "Teeth ");
        inputString = inputString.Replace("Fung", "Fang ");
        inputString = inputString.Replace(" L ", " Left");
        inputString = inputString.Replace(" R ", " Right");

        return inputString;
    }

    [System.Serializable]
    public struct BlendshapeData
    {
        public BlendshapeValue[] root;
    }

    [System.Serializable]
    public struct BlendshapeValue
    {
        public string name;
        public float value;
    }
}
