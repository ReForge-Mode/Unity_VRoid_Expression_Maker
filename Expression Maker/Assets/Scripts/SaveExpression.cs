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
        filePath = VRM10FileDialogForWindows.SaveDialog("Save Facial Expression", "Facial Expression 1" + ".fcl", "fcl");
#else
        filePath = Application.dataPath + "/default.fcl";
#endif



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

    //public void SaveFacialExpression()
    //{
    //    if (vrmMesh != null)
    //    {
    //        FileBrowser.SetFilters(false, ".fcl");
    //        //FileBrowser.SetDefaultFilter(".anim");
    //        StartCoroutine(ShowSaveDialogCoroutine());
    //    }
    //}

    //private void CreateFacialFile()
    //{
    //    // Get the blendshape count from the skinned mesh renderer
    //    int blendShapeCount = vrmMesh.sharedMesh.blendShapeCount;

    //    // Loop through all blendshapes
    //    for (int i = 0; i < blendShapeCount; i++)
    //    {
    //        // Get the current blendshape name and value
    //        string blendShapeName = vrmMesh.sharedMesh.GetBlendShapeName(i);
    //        float blendShapeValue = vrmMesh.GetBlendShapeWeight(i);

    //        // If the blendshape value is not 0, add it to the animation clip
    //        if (blendShapeValue != 0)
    //        {
    //            // Create a new curve for the blendshape
    //            AnimationCurve curve = new AnimationCurve();

    //            // Add a keyframe at frame 0 with the blendshape value
    //            Keyframe keyframe = new Keyframe(0, blendShapeValue);
    //            curve.AddKey(keyframe);

    //            // Add the curve to the animation clip, specifying the path to the skinned mesh renderer
    //            // in the child object "Face"
    //            clip.SetCurve("Face", typeof(SkinnedMeshRenderer), "blendShape." + blendShapeName, curve);
    //        }
    //    }
    //}


    //private void CreateBlendshapeAnimation()
    //{
    //    // Create a new animation clip
    //    clip = new AnimationClip();

    //    // Get the blendshape count from the skinned mesh renderer
    //    int blendShapeCount = vrmMesh.sharedMesh.blendShapeCount;

    //    // Loop through all blendshapes
    //    for (int i = 0; i < blendShapeCount; i++)
    //    {
    //        // Get the current blendshape name and value
    //        string blendShapeName = vrmMesh.sharedMesh.GetBlendShapeName(i);
    //        float blendShapeValue = vrmMesh.GetBlendShapeWeight(i);

    //        // If the blendshape value is not 0, add it to the animation clip
    //        if (blendShapeValue != 0)
    //        {
    //            // Create a new curve for the blendshape
    //            AnimationCurve curve = new AnimationCurve();

    //            // Add a keyframe at frame 0 with the blendshape value
    //            Keyframe keyframe = new Keyframe(0, blendShapeValue);
    //            curve.AddKey(keyframe);

    //            // Add the curve to the animation clip, specifying the path to the skinned mesh renderer
    //            // in the child object "Face"
    //            clip.SetCurve("Face", typeof(SkinnedMeshRenderer), "blendShape." + blendShapeName, curve);
    //        }
    //    }
    //}

    //private void SaveAnimationClip()
    //{
    //    // Set the clip to loop
    //    clip.wrapMode = WrapMode.Loop;

    //    // Save the animation clip to the root Assets folder
    //    AssetDatabase.CreateAsset(clip, "Assets/" + fileName + ".anim");

    //    // If a file path was chosen, move the animation clip to that location
    //    if (!string.IsNullOrEmpty(filePath))
    //    {
    //        File.Move("Assets/" + fileName + ".anim", filePath);
    //    }
    //}

    public void UpdateVRMMesh(GameObject target)
    {
        vrmMesh = target.transform.Find("Face").GetComponent<SkinnedMeshRenderer>();
    }

    //private IEnumerator ShowSaveDialogCoroutine()
    //{
    //    //Clear previous save files
    //    filePath = null;
    //    fileName = null;

    //    // Show a load file dialog and wait for a response from user
    //    // Load file/folder: both, Allow multiple selection: true
    //    // Initial path: default (Documents), Initial filename: empty
    //    // Title: "Load File", Submit button text: "Load"
    //    yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Save your Facial Expression", "Save");

    //    // Dialog is closed
    //    // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
    //    Debug.Log(FileBrowser.Success);

    //    if (FileBrowser.Success)
    //    {
    //        // Open a save file window to choose where to move the animation clip
    //        filePath = FileBrowser.Result[0];

    //        if (!string.IsNullOrEmpty(filePath))
    //        {
    //            var temp = Path.GetFileName(filePath);
    //            fileName = temp.Substring(0, temp.Length - 5);
    //        }

    //        GetBlendshapes();
    //        SaveToJSON();

    //        //CreateBlendshapeAnimation();
    //        //SaveAnimationClip();
    //    }
    //}

    //private void ChooseFilePath()
    //{
    //    //Clear previous save files
    //    filePath = null;
    //    fileName = null;

    //    FileBrowser.SetFilters(true, new FileBrowser.Filter("VRoid Model File", ".vrm"));
    //    StartCoroutine(ShowSaveDialogCoroutine());

    //    // Open a save file window to choose where to move the animation clip
    //    filePath = EditorUtility.SaveFilePanel("Save Facial Expression to... ", "", "Facial Expression", "anim");

    //    if (!string.IsNullOrEmpty(filePath))
    //    {
    //        var temp = Path.GetFileName(filePath);
    //        fileName = temp.Substring(0, temp.Length - 5);
    //    }
    //}
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