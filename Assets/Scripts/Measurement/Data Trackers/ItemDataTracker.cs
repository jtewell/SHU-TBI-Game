using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataTracker : MonoBehaviour
{
    private void OnEnable()
    {
        InteractablePickupItem.onPickupItemEvent.AddListener(UpdateDataPickupItem);
    }

    private void OnDisable()
    {
        InteractablePickupItem.onPickupItemEvent.RemoveListener(UpdateDataPickupItem);
    }

    public void UpdateDataPickupItem(string location)
    {
        MeasurementDataManager.Instance.itemsInteracted += location + " , ";
    }
}
