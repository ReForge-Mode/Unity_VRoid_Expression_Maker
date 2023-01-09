using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiSaveFile : MonoBehaviour
{
    public SaveAnimation saveAnimation;
    public UnityEvent onSaveSuccess;

    public void onSaveButtonClick()
    {
        var isSaveSucesss = saveAnimation.OnSaveClicked();

        if(isSaveSucesss == true)
        {
            onSaveSuccess.Invoke();
        }
    }
}
