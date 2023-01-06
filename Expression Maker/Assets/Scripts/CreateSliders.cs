using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10.VRM10Viewer;

public class CreateSliders : MonoBehaviour
{
    public GameObject sliderSetPrefab;

    // A list to store the blend shapes
    public List<string> blendShapes = new List<string>();

    public void CreateSlider()
    {
        var humanoid = GameObject.Find("VRM1");

        if (humanoid != null)
        {
            // Get the SkinnedMeshRenderer component of the humanoid
            SkinnedMeshRenderer skinnedMeshRenderer = humanoid.GetComponentInChildren<SkinnedMeshRenderer>();

            // Get the blend shapes of the SkinnedMeshRenderer
            for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                blendShapes.Add(EditName(skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i)));

                //Create Slider Set Prefabs
                var sliderSet = Instantiate(sliderSetPrefab, transform).GetComponent<SlidersSets>();
                sliderSet.blendshapeIndex = i;
                sliderSet.humanoid = skinnedMeshRenderer;

                string editedName = blendShapes[i];
                sliderSet.gameObject.name = EditName(editedName);
            }
        }
    }

    public string EditName(string input)
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
}
