using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISaveFile : MonoBehaviour
{
    public SaveExpression saveExpression;
    public UnityEvent onSaveSuccess;

    public void onSaveButtonClick()
    {
        var isSaveSucesss = saveExpression.OnSaveClicked();

        if(isSaveSucesss == true)
        {
            onSaveSuccess.Invoke();
        }
    }
}
