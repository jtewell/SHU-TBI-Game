using UnityEngine.Events;
using UnityEngine;

[System.Serializable] public class OnPickupItemEvent : UnityEvent<string> { }

public class InteractablePickupItem : MonoBehaviour
{
    public Item itemScriptableObject;
    public OnPickupItemEvent onPickupItemEvent = new OnPickupItemEvent();

    public void OnPickup()
    {
        onPickupItemEvent.Invoke(itemScriptableObject.displayName);
        InventoryManager.Instance.AddItemToInventory(itemScriptableObject);
        gameObject.SetActive(false);
    }
}