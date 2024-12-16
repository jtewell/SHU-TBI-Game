using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InteractableNPC : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private CameraController cameraController;
    private GameObject mainCamera;



    public string conversationStartNode;
    public void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = mainCamera.GetComponent<CameraController>();
    }
    public void PlayDialogue()
    {
        dialogueRunner.StartDialogue(conversationStartNode);
    }
}
