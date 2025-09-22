using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OverlayManager : MonoBehaviour
{

    [SerializeField] private GameObject header, buttons, joystick;

    private bool dialogListenerFlag = true;

    private void OnEnable()
    {
        //Subscribe to dialogue start and complete events
        if (DialogueSystem.Instance.DialogueRunner != null)
        {
            DialogueSystem.Instance.DialogueRunner.onDialogueStart.AddListener(DisableUIOnDialogStart);
            DialogueSystem.Instance.DialogueRunner.onDialogueComplete.AddListener(EnableUIOnDialogEnd);
        }
    }

    private void OnDisable()
    {
        //Unsubscribe to dialogue start and complete events
        //Check to see if the Dialogue System is still active
        if (DialogueSystem.HasInstance)
        {
            if (DialogueSystem.Instance.DialogueRunner != null)
            {
                DialogueSystem.Instance.DialogueRunner.onDialogueStart.RemoveListener(DisableUIOnDialogStart);
                DialogueSystem.Instance.DialogueRunner.onDialogueComplete.RemoveListener(EnableUIOnDialogEnd);
            }

        }

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