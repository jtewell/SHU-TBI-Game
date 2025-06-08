using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable] public class OnDoorOpenEvent : UnityEvent<string> { }

public class InteractableDoor : MonoBehaviour
{
    public string nextScene;
    public string spawnPoint;

    public static OnDoorOpenEvent onDoorOpenEvent = new OnDoorOpenEvent();

    public void SwitchScene()
    {
        onDoorOpenEvent?.Invoke(nextScene);
        SceneController.Instance.LoadScene(nextScene, spawnPoint);
    }
}
