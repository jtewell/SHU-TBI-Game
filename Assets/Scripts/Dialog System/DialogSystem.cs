using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogSystem : MonoSingleton<DialogSystem>
{
    private DialogueRunner dialogueRunner;

    private void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
    }
}
