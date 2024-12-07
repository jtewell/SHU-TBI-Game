using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCameraRoot;
    private Vector3 playeroffset = new Vector3(0, 5, 6);
    private Vector3 NPCoffset = new Vector3(-2, 2, 2);
    private GameObject NPCHead;
    public Camera MainCamera;
    public Camera NPCCamera;


    // Start is called before the first frame update
    void Start()
    {
        NPCCamera.transform.position = transform.position + NPCoffset;
        NPCCamera.enabled = false;
        MainCamera.enabled = true;

    }

    // Update is called once per frame
    void LateUpdate()
    {
           MainCamera.transform.position = player.transform.position + playeroffset; 
           MainCamera.transform.LookAt(playerCameraRoot.transform);
        
    }
    public void SwitchedScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MainCamera.transform.position = transform.position + playeroffset;
        NPCCamera.transform.position = transform.position + NPCoffset;
    }
    public void ActivateNPCCamera()
    {
        MainCamera.enabled = false;
        NPCCamera.enabled = true;
        NPCHead = GameObject.FindGameObjectWithTag("NPCHead");
        NPCCamera.transform.LookAt(NPCHead.transform);
    }
    public void DeactivateNPCCamera()
    {
        NPCCamera.enabled = false;
        MainCamera.enabled = true;
    }
}