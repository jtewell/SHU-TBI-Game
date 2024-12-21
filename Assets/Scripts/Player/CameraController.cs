using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject NPCCamera;

    void Start()
    {
        MainCamera.SetActive(true);
        NPCCamera.SetActive(false);
    }

    public void ActivateNPCCamera(Transform target)
    {
        //Turn off main camera, turn on NPC camera
        MainCamera.SetActive(false);
        NPCCamera.SetActive(true);

        //Make NPC camera look at the target
        NPCCamera.transform.LookAt(target);

        //Make the player look at the target
        transform.LookAt(target);

        //Don't affect the player's X or Z axis
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;
    }

    public void DeactivateNPCCamera()
    {
        MainCamera.SetActive(true);
        NPCCamera.SetActive(false);
    }
}