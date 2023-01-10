using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UniJSON;
using UnityEditor;
using UnityEngine;

public class FclFileImporter : MonoBehaviour
{
    public string openFileName;

    [SerializeField] private List<string> nameList;             //Contain the name of the files for naming purposes
    [SerializeField] private List<string> dirList;              //Contain the directory of the files
    [SerializeField] private List<string> jsonList;             //Contain the json parsed from the files
    [SerializeField] private List<AnimationClip> clipList;      //Contain the final clip, ready to be created as an asset

    public void OpenSingleFile()
    {
        string jsonInput = OpenFile();

        if (!string.IsNullOrEmpty(jsonInput))
        {
            BlendshapeValue[] blendshapeList = ExtractDataFromJSON(jsonInput);
            AnimationClip clip = CreateBlendshapeAnimation(blendshapeList);
            SaveAnimationClip(clip, openFileName);
        }
    }

    public void OpenBulkFolder()
    {
        OpenExpressionFolder();

        if (dirList.Count > 0)
        {
            CreateAnimationClips();
            CreateAnimationAsset();
        }
    }

    #region Folder Bulk Processing
    /// <summary>
    /// This is the function that open the windows dialogue to find the folder
    /// </summary>
    /// <returns></returns>
    private string OpenFileExplorer()
    {
        return EditorUtility.OpenFolderPanel("Find .fcl Folder", "", "fcl");
    }

    /// <summary>
    /// Open a folder, then add every valid .fcl file into the dirList and nameList
    /// </summary>
    private void OpenExpressionFolder()
    {
        string tempPath = OpenFileExplorer();
        if (tempPath != "")
        {
            DirectoryInfo dir = new DirectoryInfo(tempPath);
            FileInfo[] info = dir.GetFiles("*.fcl");

            if (info.Length > 0)
            {
                //Clear everything first
                dirList.Clear();
                nameList.Clear();

                string name = "";
                foreach (FileInfo f in info)
                {
                    dirList.Add(f.FullName);

                    name = (f.Name).Replace(".fcl", "");
                    nameList.Add(name);
                }
            }
        }
    }


    /// <summary>
    /// Using the files loaded in the dirList, create its keyframe animation curve and load it to clipList
    /// </summary>
    private void CreateAnimationClips()
    {
        clipList.Clear();

        foreach (var item in dirList)
        {
            string jsonInput = System.IO.File.ReadAllText(item);
            BlendshapeValue[] blendshapeList = ExtractDataFromJSON(jsonInput);
            AnimationClip clip = CreateBlendshapeAnimation(blendshapeList);
            clipList.Add(clip);
        }
    }

    /// <summary>
    /// Using the clips in the clipList, create an asset and store it in savePath directory
    /// </summary>
    private void CreateAnimationAsset()
    {
        string folderPath = UnityEditor.EditorUtility.SaveFolderPanel("Save .anim file", "", "Facial Expression 1");

        if (!string.IsNullOrEmpty(folderPath))
        {
            for (int i = 0; i < clipList.Count; i++)
            {
                SaveAnimationClip(clipList[i], folderPath + "/" + nameList[i] + ".anim", nameList[i]);
            }

            UnityEditor.AssetDatabase.Refresh();
        }
    }
#endregion


    #region Single File Processing
    private string OpenFile()
    {
        string openFilePath = UnityEditor.EditorUtility.OpenFilePanel("Open .fcl file", "", "fcl");
        Debug.Log(openFilePath);

        if (openFilePath.Length != 0)
        {
            ////Find the name
            var temp = Path.GetFileName(openFilePath);
            openFileName = temp.Substring(0, temp.Length - 4);

            // Use path to load file and parse JSON data
            return System.IO.File.ReadAllText(openFilePath);
        }

        return null;
    }

    private BlendshapeValue[] ExtractDataFromJSON(string jsonInput)
    {
        if (!string.IsNullOrEmpty(jsonInput))
        {
            BlendshapeValue[] blendshapeList;

            //Reset array
            blendshapeList = null;

            // Parse the JSON data
            BlendshapeData data = JsonUtility.FromJson<BlendshapeData>(jsonInput);
            blendshapeList = data.root;

            return blendshapeList;
        }

        return null;
    }

    private AnimationClip CreateBlendshapeAnimation(BlendshapeValue[] blendshapeList)
    {
        // Create a new animation clip
        AnimationClip clip = new AnimationClip();

        // Set the clip to loop
        clip.wrapMode = WrapMode.Loop;

        // Loop through all blendshapes
        for (int i = 0; i < blendshapeList.Count(); i++)
        {
            // Create a new curve for the blendshape
            AnimationCurve curve = new AnimationCurve();

            // Add a keyframe at frame 0 with the blendshape value
            Keyframe keyframe = new Keyframe(0, blendshapeList[i].value);
            curve.AddKey(keyframe);

            // Add the curve to the animation clip, specifying the path to the skinned mesh renderer
            // in the child object "Face"
            clip.SetCurve("Face", typeof(SkinnedMeshRenderer), "blendShape." + blendshapeList[i].name, curve);
        }

        return clip;
    }

    private void SaveAnimationClip(AnimationClip clip, string name)
    {
        //Choose the path
        string path = UnityEditor.EditorUtility.SaveFilePanel("Save .anim file", "", "Facial Expression 1", "anim");

        SaveAnimationClip(clip, path, name);
    }

    private void SaveAnimationClip(AnimationClip clip, string path, string name)
    {
        // Save the animation clip to the root Assets folder
        AssetDatabase.CreateAsset(clip, "Assets/" + name + ".anim");

        // If a file path was chosen, move the animation clip to that location
        if (!string.IsNullOrEmpty(path))
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Move("Assets/" + name + ".anim", path);
        }
    }
    #endregion

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
