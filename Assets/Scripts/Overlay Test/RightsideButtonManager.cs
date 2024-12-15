using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class RightsideButtonManager : MonoBehaviour
{
    public GameObject MapUI, InventoryUI;
    public Button CloseButton, CompassButton;
    public Sprite wallet, keys, umbrella; // Textures for the items to add to the inventory 
    public Image OnelineItemContainer;  // Image container for the first line of items (with RectTransforms for each item)
    public Image SecondlineItemContainer; // Image container for the second line of items
    public GameObject itemPrefab; // Prefab for an inventory item
    public GameObject player_Visual;

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


    public void OnCompassButtonClicked()
    {
        //Player.GetComponent<Player>().ShowCompass();
        Debug.Log("Compass Button Clicked");
    }
    public void OnMapButtonClicked()
    {
        //Player.GetComponent<Player>().ShowMap();
        MapUI.SetActive(true);
        Debug.Log("Map Button Clicked");
        UpdateMapState();
    }
    public void OnInventoryButtonClicked()
    {
        //Player.GetComponent<Player>().ShowInventory();
        InventoryUI.SetActive(true);
        Debug.Log("Inventory Button Clicked");
    }
    public void OnCloseButtonClicked()
    {
        MapUI.SetActive(false);
        InventoryUI.SetActive(false);
        Debug.Log("Close Button Clicked");
    }
    // get player position 
    public Vector3 GetPlayerPosition()
    {
        if (PlayerManager.Instance != null)
        {
            Vector3 playerPos = PlayerManager.Instance.GetPlayerPosition();
            //Debug.Log("Player Position from another scene: " + playerPos);
            return playerPos;
        }
        else
        {
            //Debug.LogError("PlayerManager instance is null!");
            return Vector3.zero;
        }
    }

    // get Ground Dimension
    public Vector3 GetGroundDimension()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InitializeGroundDimensions("Ground/Map");
            Vector3 groundDimensions = GameManager.Instance.GroundDimensions;
            //Vector3 groundDimension = GameManager.Instance.InitializeGroundDimensions("Ground/Map");
            //Debug.Log("Ground Dimension from another scene: " + groundDimensions);
            return groundDimensions;
        }
        else
        {
            //Debug.LogError("PlayerManager instance is null!");
            //return 999, 999, 999; // Return a default value if PlayerManager is not assigned
            return Vector3.zero;
        }
    }

    private void UpdateMapState()
    {

        // Get player's position using the new function
        Vector3 playerPos = GetPlayerPosition();
        Debug.Log("Player Position: " + playerPos);

        // Get the ground dimensions using the new function
        Vector3 groundDimension = GetGroundDimension();
        Debug.Log("Ground Dimension: " + groundDimension);

        //// Calculate the x and y position of the player icon on the map based on the player's position and the ground dimensions
        //if (groundDimension != playerPos) {
        //    playerPos.x = playerPos.x / groundDimension.x * 100;
        //    playerPos.z = playerPos.z / groundDimension.z * 100;
        //}
        //else
        //{
        //    playerPos.x = 0;
        //    playerPos.z = 0;
        //}
        float x = playerPos.x * (-1) * 3;
        float y = playerPos.z * (-1) * 3;

        // get the player_Visual object
        player_Visual = GameObject.Find("player_Visual");

        // set the position of the player_Visual as per map and as per calculation of x and y
        // here Some Sample values 
        // Ground = (200, 0, 200)
        // Player = (70, 0, 48)
        // need x,y = ??


        // set player_Visual react transform position
        player_Visual.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        float playerRotationY = PlayerManager.Instance.transform.rotation.eulerAngles.y;
        //if(playerRotationY > 90 && playerRotationY < 180)
        //{
        //    playerRotationY = playerRotationY - 180;
        //}
        //if(playerRotationY < -90 && playerRotationY > 180)
        //{
        //    playerRotationY = playerRotationY + 180;
        //}
        //if(playerRotationY > -90 && playerRotationY < 90)
        //{
        //    playerRotationY = playerRotationY * (-1);
        //}else if(playerRotationY > 270)
        //{
        //    playerRotationY = playerRotationY * (-1);
        //}
        // set the rotation of the player_Visual as per player rotation
        Debug.Log("Player Rotation Y: " + playerRotationY);
        //player_Visual.transform.rotation = Quaternion.Euler(0,0, playerRotationY);
        // log this to the console
        Debug.Log("Player icon position: " + x + ", " + y);

    }
}
