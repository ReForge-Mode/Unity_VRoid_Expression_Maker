using System.Collections;
using UnityEngine;

public class SnapCamera : MonoBehaviour
{
    public Transform target; // The game object to follow
    public Camera cam;
    public float smoothTime = 0.3f; // The smoothing time for the camera's movement
    public Vector3 offset; // The desired offset from the target
    public float scrollSensitivity = 1.0f; // The sensitivity of the scroll wheel


    private void Update()
    {
        // Smoothly follow the target
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothTime * Time.deltaTime);
        }
    }

    public void SetCamera(GameObject vrm1)
    {
        target = FindChildWithName(vrm1.transform, "J_Bip_C_Neck");
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