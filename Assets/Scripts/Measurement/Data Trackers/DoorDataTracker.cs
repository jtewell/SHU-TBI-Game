using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDataTracker : MonoBehaviour
{
    private void OnEnable()
    {
        InteractableDoor.onDoorOpenEvent.AddListener(UpdateDataDoorOpened);
    }

    private void OnDisable()
    {
        InteractableDoor.onDoorOpenEvent.RemoveListener(UpdateDataDoorOpened);
    }

    public void UpdateDataDoorOpened (string location)
    {
        MeasurementDataManager.Instance.locationsVisited += location + " , ";
    }
}
