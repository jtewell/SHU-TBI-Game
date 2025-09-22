using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//Quests are Narratives BTW

//----------------Event declarations------------------//

//Event that is invoked by this class mainly to notify the todo list that it needs to rerender itself
[System.Serializable] public class OnStepCheckedEvent : UnityEvent { }

//Event that is invoked by this class to notify that a quest has been loaded
[System.Serializable] public class OnStartQuestEvent : UnityEvent { }

//Event that is invoked by this class to notify that a quest has been completed
[System.Serializable] public class OnFinishQuestEvent : UnityEvent { }

//Event that is invoked by this class to notify that all quests has been completed (the end of the game has been reached)
[System.Serializable] public class OnFinishAllQuestsEvent : UnityEvent { }


public class QuestManager : PersistentMonoSingleton<QuestManager>
{
    //Public / Serialized fields
    [SerializeField] private Quest[] questList;

    //Static event fields that can be accessed anywhere in the game
    public static OnStepCheckedEvent onStepChecked = new OnStepCheckedEvent();
    public static OnStartQuestEvent onStartQuest = new OnStartQuestEvent();
    public static OnFinishQuestEvent onFinishQuest = new OnFinishQuestEvent();
    public static OnFinishAllQuestsEvent onFinishAllQuests = new OnFinishAllQuestsEvent();

    //Properties
    public Quest CurrentQuest
    {
        get
        {
            if (questList == null || questList.Length == 0)
            {
                return null;
            }
            return questList[currentQuestIterator];
        }
    }

    //Private fields
    private int currentQuestIterator = 0;

    public void Start()
    {
        //Reset all steps in first quest to not completed state
        questList[currentQuestIterator].ClearQuestSteps();
        onStartQuest?.Invoke();
    }

    //Explictly call this to load the next quest, which will restart the game for the next narrative
    public void LoadNextQuest()
    {
        currentQuestIterator++;

        Debug.Log("Loading next quest");

        //Check if all quests have been completed, if true then notify the game managers
        if (currentQuestIterator >= questList.Length)
        {
            Debug.Log("No more quests");
            onFinishAllQuests.Invoke();
        }

        //Else
        else
        {
            //Reset all steps to not completed state for next quest
            questList[currentQuestIterator].ClearQuestSteps();
        }
    }

    public void FinishStep(string stepId)
    {
        Debug.Log("Finishing step " + stepId);
        var step = questList[currentQuestIterator].steps.FirstOrDefault(x => x.StepId == stepId);
        if (step == null)
        {
            Debug.LogError("Step " + stepId + " does not exist!");
            return;
        }

        step.IsCompleted = true;

        onStepChecked.Invoke();

        //check if all steps have been completed
        if (questList[currentQuestIterator].steps.All(s => s.IsCompleted))
        {
            Debug.Log("all steps have been completed");

            //Tell Game objects that the current quest is finished (like the data manager)
            onFinishQuest.Invoke();
        }
    }
}