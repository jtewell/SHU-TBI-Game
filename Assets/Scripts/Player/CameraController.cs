using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera NPC_Camera;
    private GameObject player;

    void Start()
    {
        //MainCamera.SetActive(true);
        //NPCCamera.SetActive(false);

        // Get the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("Camera can't find the player in the scene!");
            return;
        }

        //Setup the player virtual camera to follow and look at the player
        playerCamera.Follow = player.transform;

        //Setup the player virtual camera to look at the player camera root child object
        Transform cameraRoot = player.transform.Find("Player Camera Root");

        if (cameraRoot == null)
        {
            Debug.Log("Camera can't find the player camera root object in the player object!");
            return;
        }

        playerCamera.LookAt = cameraRoot;
    }

    public void ActivateNPCCamera(Transform target)
    {
        //Turn off main camera, turn on NPC camera
        //MainCamera.SetActive(false);
        //NPCCamera.SetActive(true);

        NPC_Camera.Priority = 20;

        //Make NPC camera look at the target
        //NPC_Camera.transform.LookAt(target);
        NPC_Camera.LookAt = target;

        //Make the player look at the target
        //transform.LookAt(target);

        //Don't affect the player's X or Z axis
        //Vector3 rotation = transform.eulerAngles;
        //rotation.x = 0;
        //rotation.z = 0;
        //transform.eulerAngles = rotation;
    }

    public void NPCCameraMove (Transform target)
    {
        NPC_Camera.transform.position = target.position;
    }

    public void DeactivateNPCCamera()
    {
        //MainCamera.SetActive(true);
        //NPCCamera.SetActive(false);
        NPC_Camera.Priority = 5;
    }
}