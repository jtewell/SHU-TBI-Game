using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public Quest currentQuest;
    [DoNotSerialize] [CanBeNull] internal QuestUIController _questUIController;

    public void FinishStep(string stepId)
    {
        var step = currentQuest.steps.FirstOrDefault(x => x.StepId == stepId);
        if (step == null)
        {
            Debug.LogError("Step " + stepId + " does not exist!");
            return;
        }

        step.IsCompleted = true;
        _questUIController?.RenderQuest();
        
    }
}
