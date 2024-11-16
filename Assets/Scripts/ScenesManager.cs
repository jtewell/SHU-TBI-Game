using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance = null;

    public GameObject player;
    public GameObject[] doorArray;
    CameraController cameraController;
    GameObject mainCamera;

    public int currentDoorNumber;
    private Vector3 DoorOffset = new Vector3(1, 0, 0);

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (doorArray.Length == 0)
        {
            doorArray = GameObject.FindGameObjectsWithTag("Door");
        }
    }

    // Start is called before the first frame update
    

    public void LoadScene(int passedDoorNumber, string targetScene)
    {
        currentDoorNumber = passedDoorNumber;
        SceneManager.LoadScene(targetScene);
    }

    void OnLevelWasLoaded()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorArray = GameObject.FindGameObjectsWithTag("Door");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = mainCamera.GetComponent<CameraController>();

        for (int i = 0; i < doorArray.Length; i++)
        {
            if (doorArray[i].GetComponent<InteractableSwitchWorld>().doorNumber == currentDoorNumber)
            {
                player.transform.position = doorArray[i].transform.position + DoorOffset;
                cameraController.SwitchedScene();

            }
        }
    }
}
