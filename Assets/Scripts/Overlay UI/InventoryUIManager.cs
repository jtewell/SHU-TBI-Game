using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Define events to communicate UI open close events to the menu data tracker
[System.Serializable] public class OnInventoryOpenedEvent : UnityEvent { }
[System.Serializable] public class OnInventoryClosedEvent : UnityEvent { }


public class InventoryUIManager : MonoBehaviour
{
    public GameObject InventoryUI;
    public GameObject[] slots;
    public Sprite emptySprite;
    public static OnInventoryOpenedEvent onInventoryOpenedEvent = new OnInventoryOpenedEvent();
    public static OnInventoryClosedEvent onInventoryClosedEvent = new OnInventoryClosedEvent();

    public void OnInventoryButtonClicked()
    {
        InventoryUI.SetActive(true);
        onInventoryOpenedEvent?.Invoke();
    }
    public void OnCloseButtonClicked()
    {
        InventoryUI.SetActive(false);
        onInventoryClosedEvent?.Invoke();
    }

    private void OnEnable()
    {
        //Reset all slots
        for (int i = 0; i < 8; i++)
        {
            //Set the slot image sprite to nothing
            slots[i].GetComponent<Image>().sprite = emptySprite;
        }

        //go through the whole inventory and draw each item to its corresponding slot
        for (int i = 0; i < InventoryManager.Instance.InventoryList.Count; i++)
        {
            slots[i].GetComponent<Image>().sprite = InventoryManager.Instance.InventoryList[i].inventorySprite;
        }
    }
}
