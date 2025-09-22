using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

[DefaultExecutionOrder(-100)]
public class DialogueSystem : MonoSingleton <DialogueSystem>
{

    public DialogueRunner DialogueRunner
    {
        get { return _dialogueRunner; }
        private set { _dialogueRunner = value; }
    }

    [SerializeField] private DialogueRunner _dialogueRunner;

    protected override void Awake()
    {
        base.Awake();

        //If this is a duplicate then bail immediately
        if (Instance != this) return;

        //First try to use the serialized reference
        if (_dialogueRunner == null)
        {
            TryGetComponent (out _dialogueRunner);
        }

        if (_dialogueRunner == null)
        {
            _dialogueRunner = GetComponentInChildren<DialogueRunner>(true);
        }

        if (_dialogueRunner == null && transform.parent != null)
        {
            _dialogueRunner = transform.parent.GetComponentInChildren<DialogueRunner>(true);
        }

        if (_dialogueRunner == null)
        {
            _dialogueRunner = FindObjectOfType<DialogueRunner>(true);
        }

        if (_dialogueRunner == null)
        {
            Debug.LogError("DialogueSystem: DialogueRunner not found. " +
                           "Ensure exactly one DialogueRunner exists in the scene " +
                           "or assign it in the inspector.", this);
        }
        else
        {
            Debug.Log("DialogueSystem: Found DialogueRunner");
        }

    }
}
