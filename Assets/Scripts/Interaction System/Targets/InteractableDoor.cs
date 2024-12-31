using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractableDoor : MonoBehaviour
{
    public string nextScene;
    public string spawnPoint;

    public void SwitchScene()
    {
        SceneController.Instance.LoadScene(nextScene, spawnPoint);
    }
}
