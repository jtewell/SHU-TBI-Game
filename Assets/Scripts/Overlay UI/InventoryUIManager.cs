using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject InventoryUI;
    public Button CloseButton;
    public Sprite wallet, keys, umbrella; // Textures for the items to add to the inventory 
    public Image OnelineItemContainer;  // Image container for the first line of items (with RectTransforms for each item)
    public Image SecondlineItemContainer; // Image container for the second line of items
    public GameObject itemPrefab; // Prefab for an inventory item

    void Update()
    {
        // Check for key press (W for wallet)
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddItemToInventory(wallet, "wallet");
        }
        // Check for key press (K for keys)
        else if (Input.GetKeyDown(KeyCode.K))
        {
            AddItemToInventory(keys, "keys");
        }
        // Check for key press (U for umbrella)
        else if (Input.GetKeyDown(KeyCode.U))
        {
            AddItemToInventory(umbrella, "umbrella");
        }
    }

    void AddItemToInventory(Sprite itemSprite, string itemName)
    {
        // Define the fixed containers to check for the first line
        var containerNames = new string[] { "itemContainer1", "itemContainer2", "itemContainer3", "itemContainer4" };

        // Define the fixed containers to check for the second line
        var containerNames2 = new string[] { "itemContainer1Line2", "itemContainer2Line2", "itemContainer3Line2", "itemContainer4Line2" };

        // Check if the containers are valid
        if (OnelineItemContainer != null && SecondlineItemContainer != null)
        {
            // Prevent duplicates across both lines of containers
            if (!IsDuplicateItemInInventory(containerNames, itemName, OnelineItemContainer.transform) &&
                !IsDuplicateItemInInventory(containerNames2, itemName, SecondlineItemContainer.transform))
            {
                // Check for empty spaces in the first line of containers
                foreach (var containerName in containerNames)
                {
                    Transform itemContainer = OnelineItemContainer.transform.Find(containerName);
                    if (itemContainer != null && itemContainer.childCount == 0) // Check if the container is empty
                    {
                        AddItemToContainer(itemContainer, itemName, itemSprite);
                        Debug.Log(itemName + " added to " + containerName);
                        Debug.Log("Item Container: " + itemContainer);
                        Debug.Log("Sprite of the item: " + itemSprite);
                        return;
                    }
                }

                // Check for empty spaces in the second line of containers
                foreach (var containerName in containerNames2)
                {
                    Transform itemContainer = SecondlineItemContainer.transform.Find(containerName);
                    if (itemContainer != null && itemContainer.childCount == 0) // Check if the container is empty
                    {
                        AddItemToContainer(itemContainer, itemName, itemSprite);
                        Debug.Log(itemName + " added to " + containerName);
                        Debug.Log("Item Container: " + itemContainer);
                        Debug.Log("Sprite of the item: " + itemSprite);
                        return;
                    }
                }
            }
        }
        // If no empty container is found in either line
        Debug.Log("No empty container found for item " + itemName);
    }

    // Helper function to add an item to the container
    void AddItemToContainer(Transform itemContainer, string itemName, Sprite itemSprite)
    {
        // Create a new item in the container
        GameObject newItem = Instantiate(itemPrefab, itemContainer);
        Image itemImageComponent = newItem.GetComponent<Image>();

        if (itemImageComponent != null)
        {
            // Set the sprite of the item
            itemImageComponent.sprite = itemSprite;        
            // Set the size of the item
            itemImageComponent.rectTransform.sizeDelta = new Vector2(100, 100);
            // Set the position of the item
            itemImageComponent.rectTransform.anchoredPosition = new Vector2(0, 0);
        }
        // Optionally, set the name of the new item in the hierarchy
        newItem.name = itemName;
    }

    // Helper function to check for duplicate items in inventory
    bool IsDuplicateItemInInventory(string[] containerNames, string itemName, Transform container)
    {
        foreach (var containerName in containerNames)
        {
            Transform itemContainer = container.Find(containerName);
            if (itemContainer != null)
            {
                foreach (Transform child in itemContainer)
                {
                    if (child.name == itemName)
                    {
                        return true; // Duplicate found
                    }
                }
            }
        }
        return false; // No duplicates found
    }

    public void OnInventoryButtonClicked()
    {
        //Player.GetComponent<Player>().ShowInventory();
        InventoryUI.SetActive(true);
    }
    public void OnCloseButtonClicked()
    {
        InventoryUI.SetActive(false);
    }
}
