using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 playeroffset;
    private Vector3 NPCoffset = new Vector3(-2, 2, 2);
    private GameObject NPCHead;
    private DialogueRunner dialogueRunner;
    private bool active = false;
    private Quaternion LookAtPlayer = new Quaternion();

    // Start is called before the first frame update
    void Start()
    {
        playeroffset = transform.position - player.transform.position;
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        LookAtPlayer = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (active == false)
        {
            transform.position = player.transform.position + playeroffset;
            transform.rotation = LookAtPlayer;
        }

        if (dialogueRunner.IsDialogueRunning == false)
        {
            active = false;
        }
    }

    public void InteractNPC()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            NPCHead = GameObject.FindGameObjectWithTag("NPCHead");
            transform.position = player.transform.position + NPCoffset;
            transform.LookAt(NPCHead.transform);
        }
        active = true;
    }
}