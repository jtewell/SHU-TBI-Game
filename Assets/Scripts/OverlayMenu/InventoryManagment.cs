using System.Collections.Generic;
using UnityEngine;



public class InventoryManagment : MonoBehaviour
{
    private List<string> inventoryItems = new List<string>();

    public delegate void InventoryUpdatedDelegate();
    public event InventoryUpdatedDelegate OnInventoryUpdated;

    // Add an item to the inventory
    public void AddItem(string itemName)
    {
        inventoryItems.Add(itemName);
        Debug.Log($"{itemName} added to inventory.");

        // Notify listeners that the inventory has been updated
        OnInventoryUpdated?.Invoke();
    }

    // Get all items in the inventory
    public List<string> GetInventoryItems()
    {
        return inventoryItems;
    }

    // Display the current inventory (for debugging)
    public void DisplayInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in inventoryItems)
        {
            Debug.Log($"- {item}");
        }
    }
}
