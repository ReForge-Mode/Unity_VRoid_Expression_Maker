using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UndoStackManager : MonoBehaviour
{
    public int currentUndoSteps = 1;
    public string lastAction;
    public List<LogAction> logActionList;

    public UnityEvent onUndoNotAvailable;
    public UnityEvent onUndoAvailable;
    public UnityEvent onRedoNotAvailable;
    public UnityEvent onRedoAvailable;

    public void Awake()
    {
        ResetStack();
    }

    public void LogChanges(SlidersSets sliderSets, float fromValue, float toValue)
    {
        //Irregular Log: log Changes in the middle of undo
        if (logActionList[currentUndoSteps].actionType != ActionType.End)
        {
            //Check if the player has pressed one redo before. Or that we are not at the Start log
            //The only difference is whether we should delete the current log or not.

            float tempFromValue = logActionList[currentUndoSteps].fromValue;
            if (tempFromValue != -Mathf.Infinity && logActionList[currentUndoSteps].sliderSets.GetValue() == tempFromValue)
            {
                //In this context, this log has already been used for an undo.
                //The player has not done any redo (after an undo)
                //So we can safely delete every log after this, including this one
                //(i >= currentUndoSteps)

                for (int i = logActionList.Count - 2; i >= currentUndoSteps; i--)
                {
                    logActionList.RemoveAt(i);

                    if (logActionList[i - 1].actionType == ActionType.Start)
                    {
                        currentUndoSteps = 1;
                        break;
                    }
                }
            }
            else
            {
                //In this context, this log has been used for a redo.
                //So we can delete all log after this, but not this one (i > currentUndoSteps)

                for (int i = logActionList.Count - 2; i > currentUndoSteps; i--)
                {
                    logActionList.RemoveAt(i);
                }
                currentUndoSteps++;

                //If we are at the Start log, set the current undo step to 1
                if(tempFromValue == -Mathf.Infinity)
                {
                    currentUndoSteps = 1;
                }
            }
        }

        //Regular Log: Insert the changes before the end of the list
        LogAction logAction = new LogAction(ActionType.Action, sliderSets, fromValue, toValue);
        logActionList.Insert(currentUndoSteps, logAction);
        currentUndoSteps++;
        UpdateUI();
    }

    public void Undo()
    {
        //Running Undo once guarantee there will be at least one redo
        onRedoAvailable.Invoke();

        //Check if the player has not do any Redo after the redo
        if (lastAction == "Undo" || lastAction == "" ||
            logActionList[currentUndoSteps].actionType == ActionType.End)
        {
            float value = logActionList[currentUndoSteps - 1].fromValue;
            logActionList[currentUndoSteps - 1].sliderSets.SetValue(value);
        }
        //But if the player has Redo before, we just set the value to the current log,
        //no need to read the previous log
        else
        {
            float value = logActionList[currentUndoSteps].fromValue;
            logActionList[currentUndoSteps].sliderSets.SetValue(value);
        }
        currentUndoSteps--;

        //Disable the Undo UI when we reached the Start log
        if (logActionList[currentUndoSteps].actionType == ActionType.Start ||
            logActionList[currentUndoSteps - 1].actionType == ActionType.Start)
        {
            currentUndoSteps = 0;
            onUndoNotAvailable.Invoke();
        }

        lastAction = "Undo";
    }

    public void Redo()
    {
        //Running redo once guarantee there will be at least one undo
        onUndoAvailable.Invoke();

        //Check if the player has not do any Undo after the redo
        if(lastAction == "Redo" || lastAction == "" ||
           logActionList[currentUndoSteps].actionType == ActionType.Start)
        {
            float value = logActionList[currentUndoSteps + 1].toValue;
            logActionList[currentUndoSteps + 1].sliderSets.SetValue(value);
        }
        else
        //But if the player has Undo before, we just set the value to the current log,
        //no need to read the next log
        {
            float value = logActionList[currentUndoSteps].toValue;
            logActionList[currentUndoSteps].sliderSets.SetValue(value);
        }
        currentUndoSteps++;

        //Disable the Redo UI when we reached the End log
        if (logActionList[currentUndoSteps].actionType == ActionType.End ||
            logActionList[currentUndoSteps + 1].actionType == ActionType.End)
        {
            currentUndoSteps = logActionList.Count - 1;
            onRedoNotAvailable.Invoke();
        }

        lastAction = "Redo";
    }

    public void UpdateUI()
    {
        //When log is empty, disable the buttons
        if(logActionList.Count <= 2)
        {
            onUndoNotAvailable.Invoke();
            onRedoNotAvailable.Invoke();
        }
        else
        {
            onUndoAvailable.Invoke();
        }

        if(logActionList[currentUndoSteps].actionType == ActionType.End)
        {
            onRedoNotAvailable.Invoke();
        }

        if (logActionList[currentUndoSteps].actionType == ActionType.Start)
        {
            onUndoNotAvailable.Invoke();
        }
    }

    public void ResetStack()
    {
        logActionList.Clear();
        logActionList.Add(new LogAction(ActionType.Start, null, -Mathf.Infinity, -Mathf.Infinity));
        logActionList.Add(new LogAction(ActionType.End, null, -Mathf.Infinity, -Mathf.Infinity));
        currentUndoSteps = 1;

        UpdateUI();
    }
}

[System.Serializable]
public struct LogAction
{
    public ActionType actionType;
    public SlidersSets sliderSets;
    public float fromValue;
    public float toValue;

    public LogAction(ActionType actionType, SlidersSets sliderSets, float fromValue, float toValue)
    {
        this.actionType = actionType;
        this.sliderSets = sliderSets;
        this.fromValue = fromValue;
        this.toValue = toValue;
    }
}

public enum ActionType
{
    Action, Start, End
}