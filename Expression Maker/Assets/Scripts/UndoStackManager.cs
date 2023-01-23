using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class UndoStackManager : MonoBehaviour
{
    public int currentUndoSteps = -1;
    public List<LogAction> logActionList;

    public UnityEvent onUndoNotAvailable;
    public UnityEvent onUndoAvailable;
    public UnityEvent onRedoNotAvailable;
    public UnityEvent onRedoAvailable;

    public void Awake()
    {
        currentUndoSteps = -1;

        UpdateUI();
    }

    public void LogChanges(SlidersSets sliderSets, float fromValue, float toValue)
    {
        if (currentUndoSteps == -1)
        {
            logActionList.Add(new LogAction(sliderSets, fromValue, toValue));
            currentUndoSteps++;
            UpdateUI();

            return;
        }

        //Delete all undone actions beyond this point when a new action is set
        if (currentUndoSteps < logActionList.Count - 1)
        {
            //This is for when the value is on the fromValue on the log
            if (logActionList[currentUndoSteps].sliderSets.slider.value == logActionList[currentUndoSteps].fromValue)
            {
                for (int i = logActionList.Count - 1; i != currentUndoSteps - 1; i--)
                {
                    logActionList.RemoveAt(i);
                }
                logActionList.Add(new LogAction(sliderSets, fromValue, toValue));
            }
            //This is for when the value is on the toValue of the log
            else
            {
                //Delete anything beyond this point
                for (int i = logActionList.Count - 1; i != currentUndoSteps; i--)
                {
                    logActionList.RemoveAt(i);
                }
                logActionList.Add(new LogAction(sliderSets, fromValue, toValue));
                currentUndoSteps++;
            }
        }
        //But we could also be at the last step...
        else if (currentUndoSteps == logActionList.Count - 1)
        {
            //But on the fromValue instead of the toValue
            if (logActionList[currentUndoSteps].sliderSets.slider.value == logActionList[currentUndoSteps].fromValue)
            {
                logActionList[currentUndoSteps] = new LogAction(sliderSets, fromValue, toValue);
            }
            else
            {
                //Otherwise, just add as normal
                logActionList.Add(new LogAction(sliderSets, fromValue, toValue));
                currentUndoSteps++;
            }
        }

        UpdateUI();
        onRedoNotAvailable.Invoke();
    }

    public void Undo()
    {
        //Running Undo once guarantee there will be at least one redo
        onRedoAvailable.Invoke();

        float sliderValue = logActionList[currentUndoSteps].sliderSets.slider.value;

        //if the value already follows this log, move on to the next log
        //Otherwise, just rewind the value in this log
        float fromValue = logActionList[currentUndoSteps].fromValue;
        if (sliderValue == fromValue) // && currentUndoSteps > 0)
        {
            if (currentUndoSteps > 0)
            {
                currentUndoSteps--;
                float value = logActionList[currentUndoSteps].fromValue;
                logActionList[currentUndoSteps].sliderSets.SetValue(value);
            }
            else return;
        }
        else
        {
            float value = logActionList[currentUndoSteps].fromValue;
            logActionList[currentUndoSteps].sliderSets.SetValue(value);
        }

        //Disable undo UI
        if (currentUndoSteps == 0)
        {
            onUndoNotAvailable.Invoke();
        }
    }

    public void Redo()
    {
        //Running redo once guarantee there will be at least one undo
        onUndoAvailable.Invoke();

        float sliderValue = logActionList[currentUndoSteps].sliderSets.slider.value;

        //if the value already follows this log, move on to the next log
        //Otherwise, just rewind the value in this log
        float toValue = logActionList[currentUndoSteps].toValue;
        if (sliderValue == toValue) // && currentUndoSteps > 0)
        {
            if (currentUndoSteps <= (logActionList.Count - 1))
            {
                currentUndoSteps++;
                float value = logActionList[currentUndoSteps].toValue;
                logActionList[currentUndoSteps].sliderSets.SetValue(value);
            }
            else return;
        }
        else
        {
            float value = logActionList[currentUndoSteps].toValue;
            logActionList[currentUndoSteps].sliderSets.SetValue(value);
        }

        //Disable undo UI
        if (currentUndoSteps == (logActionList.Count - 1))
        {
            onRedoNotAvailable.Invoke();
        }
    }

    public void UpdateUI()
    {
        //When log is empty, disable the buttons
        if(logActionList.Count == 0)
        {
            onUndoNotAvailable.Invoke();
            onRedoNotAvailable.Invoke();
        }
        else
        {
            onUndoAvailable.Invoke();
        }
    }

    public void ResetStack()
    {
        logActionList.Clear();
        currentUndoSteps = -1;
        UpdateUI();
    }
}

[System.Serializable]
public struct LogAction
{
    public SlidersSets sliderSets;
    public float fromValue;
    public float toValue;

    public LogAction(SlidersSets sliderSets, float fromValue, float toValue)
    {
        this.sliderSets = sliderSets;
        this.fromValue = fromValue;
        this.toValue = toValue;
    }
}