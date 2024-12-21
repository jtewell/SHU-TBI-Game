using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneController : PersistentMonoSingleton<SceneController>
{
    public GameObject playerPrefab;
    public UnityEvent onSceneLoad;

    public string spawnPoint = "Bed";

    private void OnEnable()
    {
        //Subscribe to Scene Loaded Events
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        //Unsubscribe to Scene Loaded Events
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    //Callback once scene has finished loading
    void OnSceneLoad (Scene scene, LoadSceneMode mode)
    {
        //Run all Unity Event callbacks when scene has finished loading
        onSceneLoad.Invoke();

        //Spawn the player on scene load
        SpawnPlayer();

        //Enable only common and quest-specific Gameplay objects
        EnableQuestSceneObjects();
    }

    //Call when you need to load a new scene (i.e. opening a door)
    public void LoadScene (string nextScene, string spawnPoint)
    {
        this.spawnPoint = spawnPoint;
        SceneManager.LoadScene(nextScene);
    }

    private void SpawnPlayer ()
    {
        //Get a list of all spawn points in the loaded scene
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Stores the spawn point found in the scene
        GameObject foundSpawnPoint = null;

        //Search and see if there is a spawn point with the provided name in the scene
        foreach (GameObject spawnPoint in spawnPoints)
        {
            //If there is, then then store it
            if (spawnPoint.name == this.spawnPoint)
            {
                foundSpawnPoint = spawnPoint;
                break;
            }
        }

        //Set the player spawn point to (0, 0, 0) by default
        Vector3 playerSpawnPosition = new Vector3 (0, 0, 0);

        //Set the player spawn rotation to (0,0,0,0) by default
        Quaternion playerSpawnRotation = new Quaternion(0, 0, 0, 0);

        //If the spawn point was found then set the position and rotation to it
        if (foundSpawnPoint != null)
        {
            playerSpawnPosition = foundSpawnPoint.transform.position;
            playerSpawnRotation = foundSpawnPoint.transform.rotation;
        }

        //Get a reference to the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //If there is no player in the scene, then instantiate it
        if  (player == null)
        {
            Instantiate(playerPrefab, playerSpawnPosition, playerSpawnRotation);
        }

        //If there is a player in the scene, then just change its position and orientation
        else
        {
            player.gameObject.transform.position = playerSpawnPosition;
            player.gameObject.transform.rotation = playerSpawnRotation;
        }
    }

    private void EnableQuestSceneObjects()
    {
        //Get the current quest loaded in the Quest Manager
        Quest currentQuest = QuestManager.Instance.currentQuest;

        //Get the questID of the current quest
        string currentQuestID = currentQuest.questId;

        //Find the Gameplay object in the scene
        GameObject gamePlayObject = GameObject.Find("----- Gameplay -----");
        
        //Iterate through children gameplay objects
        for (int i = 0; i < gamePlayObject.transform.childCount; i++)
        {
            //Get the child at the specified index, even if deactivated
            Transform childTransform = gamePlayObject.transform.GetChild(i);

            //Activate only Common and the current quest child objects, deactivate all others
            if (childTransform.name == "Common") childTransform.gameObject.SetActive(true);
            else if (childTransform.name == currentQuestID) childTransform.gameObject.SetActive(true);
            else childTransform.gameObject.SetActive(false);

        }
    }
}
