using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniVRM10.VRM10Viewer;

public class UIButtonOpen : MonoBehaviour
{
    public VRMImporter vrmImporter;
    public UnityEvent onOpenSuccess;

    public void OpenFile()
    {
        var isSuccess = vrmImporter.OnOpenClicked();

        if (isSuccess == true)
        {
            // Call all functions or variables on the Unity Event list
            onOpenSuccess.Invoke();
        }
    }
}
