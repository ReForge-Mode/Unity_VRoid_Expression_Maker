using System.Collections;
using UnityEngine;

public class SnapCamera : MonoBehaviour
{
    public Transform target; // The game object to follow
    public Camera cam;
    public float smoothTime = 0.3f; // The smoothing time for the camera's movement

    public RectTransform leftPanel;
    public Vector3 offset;

    private void Update()
    {
        // Smoothly follow the target
        if (target != null)
        {
            //float conversionFactor = cam.orthographicSize / Screen.width;
            //float availableSpace = Screen.width * (1 - leftPanel.rect.width);
            //Vector3 offset = Vector3.right * conversionFactor * (leftPanel.rect.width + availableSpace / 2);

            transform.position = Vector3.Lerp(transform.position, target.position + Vector3.forward + offset, smoothTime * Time.deltaTime);
        }
    }

    public void SetCamera(GameObject vrm1)
    {
        target = FindChildWithName(vrm1.transform, "J_Adj_L_FaceEye");
    }

    public Transform FindChildWithName(Transform target, string name)
    {
        Transform[] children = target.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
}