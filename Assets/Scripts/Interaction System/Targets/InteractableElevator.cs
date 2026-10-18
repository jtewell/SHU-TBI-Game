using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FloorLevel
{
    Lobby = 0,
    FirstFloor = 1,
    SecondFloor = 2,
    ThirdFloor = 3
}

[System.Serializable]
public class OnElevatorOpenEvent : UnityEvent<string> { }

public class InteractableElevator : MonoBehaviour
{
    [System.Serializable]
    public struct Floor
    {
        public FloorLevel floorLevel;
        public string sceneName;
        public string spawnPoint;
        public Button floorButton;
    }

    [Header("Floors")]
    public List<Floor> floors = new List<Floor>();

    [Header("Current Floor")]
    public FloorLevel currentFloor = FloorLevel.Lobby;

    public static OnElevatorOpenEvent onDoorOpenEvent = new OnElevatorOpenEvent();

    [Header("Elevator UI")]
    public GameObject elevatorUIPanel;

    // =============================
    // INTERACT
    // =============================
   

    // =============================
    // OPEN UI
    // =============================
    public void OpenElevatorUI()
    {
        if (elevatorUIPanel != null)
        {
            elevatorUIPanel.SetActive(true);

           

            SetupButtons();
        }
    }

    // =============================
    // DYNAMIC BUTTON SETUP
    // =============================
    void SetupButtons()
    {
        foreach (var floor in floors)
        {
            if (floor.floorButton == null)
                continue;

            Button btn = floor.floorButton;
            btn.onClick.RemoveAllListeners();

            if (floor.floorLevel == currentFloor)
            {
                Floor lobbyFloor = floors.Find(f => f.floorLevel == FloorLevel.Lobby);

                if (lobbyFloor.floorButton != null)
                {
                    SetButton(btn, lobbyFloor);
                }
            }
            else
            {
                SetButton(btn, floor);
            }
        }
    }

    // =============================
    // BUTTON ASSIGN HELPER
    // =============================
    void SetButton(Button btn, Floor targetFloor)
    {
        Text txt = btn.GetComponentInChildren<Text>();
        if (txt != null)
        {
            txt.text = FormatFloorName(targetFloor.floorLevel);
        }

        btn.onClick.AddListener(() =>
        {
            GoToFloorByLevel(targetFloor.floorLevel);
        });
    }

    // =============================
    // FIND FLOOR BY ENUM
    // =============================
    void GoToFloorByLevel(FloorLevel level)
    {
        Floor selectedFloor = floors.Find(f => f.floorLevel == level);

        if (selectedFloor.floorLevel == currentFloor)
        {
            Debug.Log("Already on this floor.");
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
    // FORMAT NAME
    // =============================
    string FormatFloorName(FloorLevel level)
    {
        switch (level)
        {
            case FloorLevel.Lobby: return "Lobby";
            case FloorLevel.FirstFloor: return "First Floor";
            case FloorLevel.SecondFloor: return "Second Floor";
            case FloorLevel.ThirdFloor: return "Third Floor";
        }
        return level.ToString();
    }

    // =============================
    // KEEP (unused but safe)
    // =============================
    public void GoToFloor(int floorIndex)
    {
        if (floorIndex < 0 || floorIndex >= floors.Count)
            return;

        GoToFloorByLevel(floors[floorIndex].floorLevel);
    }

    // =============================
    // CLOSE UI
    // =============================
    public void CloseElevatorUI()
    {
        if (elevatorUIPanel != null)
        {
            elevatorUIPanel.SetActive(false);
        }
    }
}