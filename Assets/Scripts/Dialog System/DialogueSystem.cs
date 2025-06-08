using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class OnDialogueSystemInitialized : UnityEvent { }

public class DialogueSystem : MonoSingleton <DialogueSystem>
{

   // public static OnDialogueSystemInitialized onDialogSystemInitialized = new OnDialogSystemInitialized ();

    public DialogueRunner DialogueRunner
    {
        get { return dialogueRunner; }
    }

    private DialogueRunner dialogueRunner;

    protected override void Awake()
    {
        base.Awake();
        dialogueRunner = GetComponent<DialogueRunner>();
        //onDialogSystemInitialized?.Invoke();
    }
}
