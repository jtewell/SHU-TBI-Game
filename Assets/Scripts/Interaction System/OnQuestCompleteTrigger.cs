using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

//This class handles when a quest event is boradcasted by the QuestManager.
//It simply displays a message.
//When the user has clicked through it, it then signals to the load the next quest to the QuestManager

public class OnQuestCompleteTrigger : MonoBehaviour
{
    //Public / Serialized fields
    [SerializeField] private string dialogueCompletionNode = "Quest0_Complete";

    //Private fields
    private DialogueRunner dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        //Get the dialog runner
        DialogueSystem dialogueSystem = DialogueSystem.Instance;

        if (dialogueSystem == null)
        {
            Debug.Log("No Dialog System object in scene!");
            return;
        }

        dialogueRunner = dialogueSystem.GetComponent<DialogueRunner>();
    }

    private void OnEnable()
    {
        QuestManager.onFinishQuest.AddListener(DisplayFinishMessage);
    }

    private void OnDisable()
    {
        QuestManager.onFinishQuest.RemoveListener(DisplayFinishMessage);
    }

    private void DisplayFinishMessage ()
    {
        StartCoroutine(DisplayFinishMessageRoutine());
    }

    private IEnumerator DisplayFinishMessageRoutine ()
    {
        if (dialogueRunner != null)
        {

            if (dialogueRunner.IsDialogueRunning)
            {
                Debug.Log("Stopping dialogue");
                //dialogueRunner.Stop();

                // wait until the old dialogue is fully finished
                yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
            }

            QuestManager.onFinishQuest.RemoveListener(DisplayFinishMessage);

            dialogueRunner.StartDialogue(dialogueCompletionNode);
            dialogueRunner.onNodeComplete.AddListener(OnDialogueNodeComplete);
            Debug.Log("Finished playing final message");
        }
    }

    private void OnDialogueNodeComplete (string nodeTitle)
    {
        dialogueRunner.onNodeComplete.RemoveListener(OnDialogueNodeComplete);

        if (nodeTitle.Equals(dialogueCompletionNode))
        {
            //User is ready for the next narrative, load it!
            QuestManager.Instance.LoadNextQuest();
        }
    }


}
