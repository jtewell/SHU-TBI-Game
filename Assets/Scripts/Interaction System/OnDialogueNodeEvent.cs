using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class OnDialogueNodeEvent : MonoBehaviour
{
    [Header("Dialogue Node Name To Listen For")]
    public string dialogueNodeTitle = "Dialog node title";

    [Space]

    [Header("Dialogue Node Start Event Section")]
    public bool startIsOneShot = false;
    public UnityEvent onDialogueNodeStartEvent;
    
    [Space]

    [Header("Dialogue Node Complete Event Section")]
    public bool completeIsOneShot = false;
    public UnityEvent onDialogueNodeCompleteEvent;

    private bool _startHasBeenTriggered = false;
    private bool _completeHasBeenTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get the dialogue object in the scene
        GameObject dialogueSystem = GameObject.Find("Dialogue System");

        if (dialogueSystem != null)
        {
            //Get a reference to the dialogue component
            DialogueRunner dialogueRunner = dialogueSystem.transform.GetComponent<DialogueRunner>();

            //Subscribe to dialogue node start and event events
            dialogueRunner.onNodeStart.AddListener(OnDialogueNodeStart);
            dialogueRunner.onNodeComplete.AddListener(OnDialogueNodeComplete);
        }

        else
        {
            Debug.Log("Can't find Dialogue System object in scene");
        }
    }

    void OnDialogueNodeStart (string nodeTitle)
    {
        //Ignore more than one event call
        if (startIsOneShot == true && _startHasBeenTriggered == true) return;

        //Ignore if the node title doesn't match
        if (dialogueNodeTitle.Equals(nodeTitle) == false) return;

        //Invoke Unity Event callbacks
        onDialogueNodeStartEvent.Invoke();

        //Set flag for one shot events
        _startHasBeenTriggered = true;
    }

    void OnDialogueNodeComplete (string nodeTitle)
    {
        //Ignore more than one event call
        if (completeIsOneShot == true && _completeHasBeenTriggered == true) return;

        //Ignore if the node title doesn't match
        if (dialogueNodeTitle.Equals(nodeTitle) == false) return;

        //Invoke Unity Event callbacks
        onDialogueNodeCompleteEvent.Invoke();

        //Set flag for one shot events
        _completeHasBeenTriggered = true;
    }
}
