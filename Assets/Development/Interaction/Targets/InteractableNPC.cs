using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InteractableNPC : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    CameraController cameraController;
    GameObject mainCamera;



    public string conversationStartNode;
    public void Start()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = mainCamera.GetComponent<CameraController>();
    }
    private void PlayDialogue()
    {
        dialogueRunner.StartDialogue(conversationStartNode);
        cameraController.InteractNPC();
    }
}
