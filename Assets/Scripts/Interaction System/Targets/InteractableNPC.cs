using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

[System.Serializable] public class OnNPCInteractEvent : UnityEvent<string> { }

public class InteractableNPC : MonoBehaviour
{
    public string conversationStartNode;
    public string nameOfNPC;

    private DialogueRunner dialogueRunner;
    private CameraController cameraController;
    private GameObject mainCamera;
    public static OnNPCInteractEvent onNPCInteractEvent = new OnNPCInteractEvent();

    public virtual void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = mainCamera.GetComponent<CameraController>();
    }
    public virtual void PlayDialogue()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        onNPCInteractEvent?.Invoke(nameOfNPC);
        dialogueRunner.StartDialogue(conversationStartNode);
    }
}
