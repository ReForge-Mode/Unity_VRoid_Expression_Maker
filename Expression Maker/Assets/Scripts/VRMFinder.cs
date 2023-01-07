using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

//This class will find VRM model that has been loaded every second.
//It will then inform other scripts through event broadcast
public class VRMFinder : MonoBehaviour
{
    public UnityEvent onEventTriggered;
    public SnapCamera snapCamera;
    public SaveAnimation saveAnimation;
    public CreateSliders createSliders;

    public GameObject vrm1;

    public void Awake()
    {
        InvokeRepeating("SearchVRM1", 0f, 1f);
    }

    public void TriggerEvent()
    {
        // Call all functions or variables on the Unity Event list
        onEventTriggered.Invoke();
    }

    public void SearchVRM1()
    {
        if (vrm1 == null)
        {
            //Debug.Log("Searching...");

            vrm1 = GameObject.Find("VRM1");
            if (vrm1 != null)
            {
                //Debug.Log("Target Found!");
                TriggerEvent();
                snapCamera.SetCamera(vrm1);
                saveAnimation.UpdateVRMMesh(vrm1.gameObject);
                createSliders.CreateSlider(vrm1);
            }
        }
    }
}
