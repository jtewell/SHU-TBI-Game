using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FloorLevel
{
    FirstFloor,
    SecondFloor,
    ThirdFloor
}

[System.Serializable]
public class OnElevatorOpenEvent : UnityEvent<string> { }

public class InteractableElevator : MonoBehaviour
{
    [System.Serializable]
    public struct Floor
    {
        public FloorLevel floorLevel;   // First / Second / Third
        public string sceneName;
        public string spawnPoint;
        public Button floorButton;
    }

    [Header("Floors")]
    public List<Floor> floors = new List<Floor>();

    [Header("Current Floor")]
    public FloorLevel currentFloor;

    public static OnElevatorOpenEvent onDoorOpenEvent = new OnElevatorOpenEvent();

    [Header("Elevator UI")]
    public GameObject elevatorUIPanel;

    // =============================
    // INTERACT
    // =============================
    public void Interact()
    {
        OpenElevatorUI();
    }

    private void OnMouseDown()
    {
        OpenElevatorUI();
    }

    // =============================
    // OPEN UI
    // =============================
    public void OpenElevatorUI()
    {
        if (elevatorUIPanel != null)
        {
            elevatorUIPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UpdateButtonStates();
        }
    }

    // =============================
    // FLOOR SELECT
    // =============================
    public void GoToFloor(int floorIndex)
    {
        if (floorIndex < 0 || floorIndex >= floors.Count)
        {
            Debug.LogWarning("Invalid floor index!");
            return;
        }

        Floor selectedFloor = floors[floorIndex];

        // 🚫 Prevent selecting same floor
        if (selectedFloor.floorLevel == currentFloor)
        {
            Debug.Log("You are already on this floor.");
            return;
        }

        currentFloor = selectedFloor.floorLevel;

        onDoorOpenEvent?.Invoke(selectedFloor.sceneName);

        SceneController.Instance.LoadScene(
            selectedFloor.sceneName,
            selectedFloor.spawnPoint
        );

        CloseElevatorUI();
    }

    // =============================
    // DISABLE CURRENT FLOOR BUTTON
    // =============================
    void UpdateButtonStates()
    {
        for (int i = 0; i < floors.Count; i++)
        {
            if (floors[i].floorButton != null)
            {
                floors[i].floorButton.interactable =
                    floors[i].floorLevel != currentFloor;
            }
        }
    }

    // =============================
    // CLOSE UI
    // =============================
    public void CloseElevatorUI()
    {
        if (elevatorUIPanel != null)
        {
            elevatorUIPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}