using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    //public GameObject player;
    //public GameObject playerCameraRoot;
    //private Vector3 playeroffset = new Vector3(0, 5, 6);
    //private Vector3 NPCoffset = new Vector3(-2, 2, 2);
    //private GameObject NPCHead;

    private Camera MainCamera;
    private Camera NPCCamera;


    // Start is called before the first frame update
    void Start()
    {
        //Get the camera references
        MainCamera = transform.Find("MainCamera").GetComponent<Camera>();
        NPCCamera = transform.Find("DialogCamera").GetComponent<Camera>();

        //NPCCamera.transform.position = transform.position + NPCoffset;
        NPCCamera.enabled = false;
        MainCamera.enabled = true;

    }

    public void SwitchedScene()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //MainCamera.transform.position = transform.position + playeroffset;
        //NPCCamera.transform.position = transform.position + NPCoffset;
    }

    public void ActivateNPCCamera()
    {
        MainCamera.enabled = false;
        NPCCamera.enabled = true;
        //NPCHead = GameObject.FindGameObjectWithTag("NPCHead");
        //NPCCamera.transform.LookAt(NPCHead.transform);
    }

    public void DeactivateNPCCamera()
    {
        NPCCamera.enabled = false;
        MainCamera.enabled = true;
    }
}