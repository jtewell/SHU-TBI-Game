using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OverlayManager : MonoBehaviour
{

    [SerializeField] private GameObject header, buttons, joystick;

    private bool dialogListenerFlag = true;

    private void Start()
    {
        //Get a reference to the Dialog System Singleton's Dialogue Runner component
        DialogueRunner dialogueRunner = DialogSystem.Instance.GetComponent<DialogueRunner>();

        //Subscribe to dialogue start and complete events
        dialogueRunner.onDialogueStart.AddListener(DisableUIOnDialogStart);
        dialogueRunner.onDialogueComplete.AddListener(EnableUIOnDialogEnd);
    }

    public void DisableUIOnDialogStart ()
    {
        if (dialogListenerFlag == false) return;

        header.SetActive(false);
        buttons.SetActive(false);
        joystick.SetActive(false);
        
    }

    public void EnableUIOnDialogEnd ()
    {
        if (dialogListenerFlag == false) return;

        header.SetActive(true);
        buttons.SetActive(true);
        joystick.SetActive(true);
    }

    public void SetDialogListenerFlag (bool flag)
    {
        dialogListenerFlag = flag;
    }
}