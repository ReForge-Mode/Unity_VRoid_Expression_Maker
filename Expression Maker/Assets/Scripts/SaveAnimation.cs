using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SaveAnimation : MonoBehaviour
{
    public SkinnedMeshRenderer vrmMesh;
    private AnimationClip clip;
    public string fileName = "Facial Expression";
    public string filePath;



    public void SaveFacialExpression()
    {
        if (vrmMesh != null)
        {
            ChooseFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                CreateBlendshapeAnimation();
                SaveAnimationClip();
            }
        }
    }

    private void ChooseFilePath()
    {
        //Clear previous save files
        filePath = null;
        fileName = null;

        // Open a save file window to choose where to move the animation clip
        filePath = EditorUtility.SaveFilePanel("Save Facial Expression to... ", "", "Facial Expression", "anim");

        if (!string.IsNullOrEmpty(filePath))
        {
            var temp = Path.GetFileName(filePath);
            fileName = temp.Substring(0, temp.Length - 5);
        }
    }

    private void CreateBlendshapeAnimation()
    {
        // Create a new animation clip
        clip = new AnimationClip();

        // Get the blendshape count from the skinned mesh renderer
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
                // Create a new curve for the blendshape
                AnimationCurve curve = new AnimationCurve();

                // Add a keyframe at frame 0 with the blendshape value
                Keyframe keyframe = new Keyframe(0, blendShapeValue);
                curve.AddKey(keyframe);

                // Add the curve to the animation clip, specifying the path to the skinned mesh renderer
                // in the child object "Face"
                clip.SetCurve("Face", typeof(SkinnedMeshRenderer), "blendShape." + blendShapeName, curve);
            }
        }
    }

    private void SaveAnimationClip()
    {
        // Set the clip to loop
        clip.wrapMode = WrapMode.Loop;

        // Save the animation clip to the root Assets folder
        AssetDatabase.CreateAsset(clip, "Assets/" + fileName + ".anim");
        

        // If a file path was chosen, move the animation clip to that location
        if (!string.IsNullOrEmpty(filePath))
        {
            File.Move("Assets/" + fileName + ".anim", filePath);
        }
    }




    public void UpdateVRMMesh(GameObject target)
    {
        vrmMesh = target.transform.Find("Face").GetComponent<SkinnedMeshRenderer>();
    }
}
