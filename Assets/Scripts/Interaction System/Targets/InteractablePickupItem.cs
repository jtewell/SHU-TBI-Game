using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class OnPickupItemEvent : UnityEvent<string> { }

public class InteractablePickupItem : MonoBehaviour
{
    public Item itemScriptableObject;

    public static OnPickupItemEvent onPickupItemEvent = new OnPickupItemEvent();

    public void onPickup()
    {
        onPickupItemEvent?.Invoke(itemScriptableObject.displayName);
        InventoryManager.Instance.AddItemToInventory(itemScriptableObject);
        Destroy(gameObject);
    }


}
