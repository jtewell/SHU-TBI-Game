using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : PersistentMonoSingleton<QuestManager>
{
    public Quest currentQuest;
    public UnityEvent onFinishStep;

    public void LoadQuest (Quest newQuest)
    {
        //Load the new Quest scriptable object
        currentQuest = newQuest;

        //Reset all steps to not completed
        foreach (var step in currentQuest.steps)
        {
            step.IsCompleted = false;
        }
    }

    public void FinishStep(string stepId)
    {
        var step = currentQuest.steps.FirstOrDefault(x => x.StepId == stepId);
        if (step == null)
        {
            Debug.LogError("Step " + stepId + " does not exist!");
            return;
        }

        step.IsCompleted = true;

        onFinishStep.Invoke();

    }
}
