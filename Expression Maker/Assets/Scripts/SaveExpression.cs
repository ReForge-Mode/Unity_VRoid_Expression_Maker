using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;
using UniVRM10.VRM10Viewer;
using Unity.VisualScripting;
using System.Linq;

public class SaveExpression : MonoBehaviour
{
    public SkinnedMeshRenderer vrmMesh;
    public ScreenshotTaker screenshot;
    public string fileName = "Facial Expression";
    public string filePath;

    public List<BlendshapeValue> blendshapeList;

    public bool OnSaveClicked()
    {
        //Clear previous save files
        filePath = null;
        fileName = null;

#if UNITY_EDITOR
        filePath = EditorUtility.SaveFilePanel("Save Facial Expression", "", "Facial Expression 1", "fcl");
#elif UNITY_STANDALONE_WIN
        filePath = VRM10FileDialogForWindows.SaveDialog("Save Facial Expression", "Facial Expression 1" + ".fcl");
#else
        filePath = Application.persistentDataPath + "/" + fileName + ".fcl";
#endif

        //Analyze filePath and add extension if possible
        if (filePath.Length > 0 && filePath.Substring(filePath.Length - 4, 4) != ".fcl")
        {
            filePath += ".fcl";
        }

        Debug.Log(filePath);

        if (!string.IsNullOrEmpty(filePath))
        {
            var temp = Path.GetFileName(filePath);
            fileName = temp.Substring(0, temp.Length - 5);

            GetBlendshapes();
            SaveToJSON();

            screenshot.TakeScreenshot(filePath.Substring(0, filePath.Length - 4));
            return true;
        }

        return false;
    }



    private void GetBlendshapes()
    {
        blendshapeList.Clear();

        int blendShapeCount = vrmMesh.sharedMesh.blendShapeCount;

        // Loop through all blendshapes
        for (int i = 0; i < blendShapeCount; i++)
        {
            // Get the current blendshape name and value
            string blendShapeName = vrmMesh.sharedMesh.GetBlendShapeName(i);
            float blendShapeValue = vrmMesh.GetBlendShapeWeight(i);

            // If the blendshape value is not 0, add it to the animation clip
            if (blendShapeValue != 0)
            {
                blendshapeList.Add(new BlendshapeValue(blendShapeName, Mathf.Round(blendShapeValue)));
            }
        }
    }

    private void SaveToJSON()

    {
        //Because Unity doesn't allow serializing JSON with list directly as its root
        //I have to use a class that contains the list
        FacialFile facialFile = new FacialFile(blendshapeList);

        // Serialize the list to a JSON string.
        string json = JsonUtility.ToJson(facialFile);

        // Save the JSON string to a file.
        System.IO.File.WriteAllText(filePath, json);
    }

    public void UpdateVRMMesh(GameObject target)
    {
        vrmMesh = target.transform.Find("Face").GetComponent<SkinnedMeshRenderer>();
    }
}

[System.Serializable]
public class FacialFile
{
    public List<BlendshapeValue> root;

    public FacialFile(List<BlendshapeValue> blendshapeList)
    {
        this.root = blendshapeList;
    }
}


[System.Serializable]
public class BlendshapeValue
{
    public string name;
    public float value;

    public BlendshapeValue(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}