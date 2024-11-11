using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 playeroffset = new Vector3(0, 7, 3);
    private Vector3 NPCoffset = new Vector3(-2, 2, 2);
    private GameObject NPCHead;
    public DialogueRunner dialogueRunner;
    private bool dialogCamActive = false;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
       // playeroffset = transform.position - player.transform.position;
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dialogueRunner.IsDialogueRunning== false)
        {
            transform.position = player.transform.position + playeroffset;
            transform.LookAt(player.transform);
        }
        
    }

    /*public void OnDialogFinish()
    {
        dialogCamActive = false;
    } */

    public void SwitchedScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + playeroffset;
    }
    public void InteractNPC()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            NPCHead = GameObject.FindGameObjectWithTag("NPCHead");
            transform.position = player.transform.position + NPCoffset;
            transform.LookAt(NPCHead.transform);
        }
       // dialogCamActive = true;
    }
}