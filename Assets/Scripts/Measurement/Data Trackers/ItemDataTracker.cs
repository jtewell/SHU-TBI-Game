using UnityEngine;

public class ItemDataTracker : MonoBehaviour
{
    private InteractablePickupItem _owner;

    private void Awake()
    {
        _owner = GetComponent<InteractablePickupItem>();
        _owner.onPickupItemEvent.AddListener(UpdateDataPickupItem);
    }

    private void OnDestroy()
    {
        if (_owner != null)
            _owner.onPickupItemEvent.RemoveListener(UpdateDataPickupItem);
    }

    private void UpdateDataPickupItem(string location)
    {
        MeasurementDataManager.Instance.itemsInteracted += location + " , ";
    }
}
