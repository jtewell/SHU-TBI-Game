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

    private DialogueRunner _dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        //Get the dialogue object in the scene
        GameObject dialogueSystem = GameObject.Find("Dialogue System");

        if (dialogueSystem != null)
        {
            //Get a reference to the dialogue component
            _dialogueRunner = dialogueSystem.transform.GetComponent<DialogueRunner>();

            //Subscribe to dialogue node start and event events
            _dialogueRunner.onNodeStart.AddListener(OnDialogueNodeStart);
            _dialogueRunner.onNodeComplete.AddListener(OnDialogueNodeComplete);
        }

        else
        {
            Debug.Log("Can't find Dialogue System object in scene");
        }
    }

    private void OnEnable()
    {
        if (_dialogueRunner != null)
        {
            //Assume listeners have been added already by start
            _dialogueRunner.onNodeStart.RemoveListener(OnDialogueNodeStart);
            _dialogueRunner.onNodeComplete.RemoveListener(OnDialogueNodeComplete);

            //Then add the listeners
            _dialogueRunner.onNodeStart.AddListener(OnDialogueNodeStart);
            _dialogueRunner.onNodeComplete.AddListener(OnDialogueNodeComplete);
        }

    }

    private void OnDisable()
    {
        if (_dialogueRunner != null)
        {
            //Remove listeners
            _dialogueRunner.onNodeStart.RemoveListener(OnDialogueNodeStart);
            _dialogueRunner.onNodeComplete.RemoveListener(OnDialogueNodeComplete);
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

        //If a callback is a new dialogue node, Unity will crash so wait a bit
        StartCoroutine(StartCallbacksWhenReady());
    }

    IEnumerator StartCallbacksWhenReady()
    {
        // Wait until Yarn reports no dialogue running
        while (_dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        //Invoke Unity Event callbacks
        onDialogueNodeCompleteEvent.Invoke();

        //Set flag for one shot events
        _completeHasBeenTriggered = true;
    }
}
