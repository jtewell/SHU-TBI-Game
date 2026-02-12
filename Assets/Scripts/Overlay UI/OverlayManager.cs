using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class OverlayManager : MonoBehaviour
{

    [SerializeField] private GameObject header, buttons, joystick, joystickMove, sprintButton;
    [SerializeField] private bool leftSideJoystick = true;
    private Vector3 joystickScale, joystickMoveScale, sprintButtonScale;
    //private Vector3 joystickLeftSide = new Vector3(83.16f,0f,59.40f);
    //private Vector3 joystickRightSide = new 

    private bool dialogListenerFlag = true;

    private void OnEnable()
    {
        //Subscribe to dialogue start and complete events
        if (DialogueSystem.Instance.DialogueRunner != null)
        {
            DialogueSystem.Instance.DialogueRunner.onDialogueStart.AddListener(DisableUIOnDialogStart);
            DialogueSystem.Instance.DialogueRunner.onDialogueComplete.AddListener(EnableUIOnDialogEnd);
        }
        invertButtonSides();
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

    public void invertButtonSides()
    {
        joystickScale = new Vector3(joystick.transform.localScale.x,joystick.transform.localScale.y,joystick.transform.localScale.z);
        joystickScale.x = joystick.transform.localScale.x * (leftSideJoystick ? 1 : -1);
        joystick.transform.localScale = joystickScale;

        joystickMoveScale = new Vector3(joystickMove.transform.localScale.x,joystickMove.transform.localScale.y,joystickMove.transform.localScale.z);
        joystickMoveScale.x = joystickMove.transform.localScale.x * (leftSideJoystick ? 1 : -1);
        joystickMove.transform.localScale = joystickMoveScale;
        
        sprintButtonScale = new Vector3(sprintButton.transform.localScale.x,sprintButton.transform.localScale.y,sprintButton.transform.localScale.z);
        sprintButtonScale.x = sprintButton.transform.localScale.x * (leftSideJoystick ? 1 : -1);
        sprintButton.transform.localScale = sprintButtonScale;


    }
}