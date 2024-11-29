using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using StarterAssets; // Import the StarterAssets namespace


public class rightsideMenu : MonoBehaviour
{
    public GameObject Player;

    //public GameObject inventoryManagerObject; // Reference to the GameObject with InventoryManagement script
    // Reference to the child's script 
    private InventoryManagment inventoryManager;


    private VisualElement  overlayMenu, rightMenuContainer, OnelineItemContainer, SecondlineItemContainer, inventoryContainer, 
        mapContainer, detailed_mapVisualElement, closeMapButtonContainer, _playerRepresentation;
    private Button mapButton, inventoryButton, closeButton, closeButtonMap;
    private bool MapFaded;

    public float fullMultiplyer = 10f; // Example multiplier value

    public Texture2D wallet, keys, umbrella; // Textures for the items to add to the inventory 
    // Start is called before the first frame update
    void Start()
    {
        // Get the root of the overlay UI
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Get the overlay menu and right menu container
        overlayMenu = root.Q<VisualElement>("overlayMenu");
        rightMenuContainer = root.Q<VisualElement>("rightMenuContainer");
        inventoryContainer = root.Q<VisualElement>("inventoryContainer");
        mapContainer = root.Q<VisualElement>("mapContainer");
        detailed_mapVisualElement = root.Q<VisualElement>("detailed_mapVisualElement");
        OnelineItemContainer = root.Q<VisualElement>("OnelineItemContainer");
        SecondlineItemContainer = root.Q<VisualElement>("SecondlineItemContainer");
        //_playerRepresentation = root.Q<VisualElement>("player-icon");
        _playerRepresentation = root.Q<VisualElement>("player_Visual");


        mapButton = root.Q<Button>("mapButton");
        inventoryButton = root.Q<Button>("inventoryButton");
        closeButton = root.Q<Button>("closeButton");
        closeButtonMap = root.Q<Button>("closeButtonMap");

        mapContainer.visible = false;
        //check if the overlaymenu is available and the right menu container is available otherwise show error
        if (overlayMenu == null)
        {
            Debug.LogError("Overlay menu container not found");
            return;
        }
        if (rightMenuContainer == null)
        {
            Debug.LogError("Right menu container not found");
            return;
        }
        if (inventoryContainer == null)
        {
            Debug.LogError("Inventory container not found");
            return;
        }
        

        // Ensure inventoryContainer is initially hidden
        inventoryContainer.visible = false;

        //register the click event for the inventory button
        // Add a click event listener to the button
        if (inventoryButton != null)
        {
            inventoryButton.clicked += OnInventoryAccessClick;
        }
        else
        {
            Debug.LogError("Inventory access button not found");
        }
        closeButton.SetEnabled(true); // Ensure the button is enabled

        //Add a click event listener to the close button
        if (closeButton != null)
        {
            Debug.Log("Close button found");
            closeButton.clicked += OnCloseButtonClick;
        }
        else
        {
            Debug.LogError("Close button not found");
        }
        closeButtonMap.SetEnabled(true); // Ensure the button is enabled

        if (closeButtonMap != null)
        {
            Debug.Log("Close button found");
            closeButtonMap.clicked += () =>
            {
                Debug.Log("Close button clicked");
                // Hide the map container
                mapContainer.visible = false;

                detailed_mapVisualElement.visible = false;
            };
        }
        else
        {
            Debug.LogError("Close button not found");
        }


        if (mapButton == null)
        {
            Debug.LogError("Map button not found");
        }
        else
        {
            mapButton.clicked += () =>
            {
                // Show the map container
                Debug.Log("Map button clicked");
                mapContainer.visible = true;
                detailed_mapVisualElement.visible = true;
                // Hide the inventory container
                //inventoryContainer.visible = false;
                // Initialize or update the map state when it is opened
                //UpdateMapState();
            };
        }
        if (OnelineItemContainer == null)
        {
            Debug.LogError("OnelineItemContainer not found");
        }

        if (SecondlineItemContainer == null)
        {
            Debug.LogError("SecondlineItemContainer container not found");
        }
       

    }
    private void OnCloseButtonClick()
    {
        // Hide the map container
        inventoryContainer.visible = false;
    }
    private void OnInventoryAccessClick()
    {
        //make inventoryContainer visible
        inventoryContainer.visible = !inventoryContainer.visible;
        Debug.Log("Inventory UI populated from child GameObject");
    }
    // Update is called once per frame 
    void Update()
    {
        // Check for key press (W for wallet)
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddItemToInventory(wallet,"wallet");
        }
        // Check for key press (K for keys)
        else if (Input.GetKeyDown(KeyCode.K))
        {
            AddItemToInventory(keys,"keys");
        }
        // Check for key press (U for umbrella)
        else if (Input.GetKeyDown(KeyCode.U))
        {
            AddItemToInventory(umbrella, "umbrella");
        }
        UpdateMapState();
    }

    // Public method to add an item to the inventory
    public void AddItemToInventory(Texture2D itemImage, string itemName)
    {
        // Define the fixed containers to check for the first line
        var containerNames = new string[]
        {
            "item1Container", "item2Container", "item3Container", "item4Container"
        };

        // Define the fixed containers to check for the second line
        var containerNames2 = new string[]
        {
            "item1ContainerLine2", "item2ContainerLine2", "item3ContainerLine2", "item4ContainerLine2"
        };

        // Check the first line of containers
        if (OnelineItemContainer != null && SecondlineItemContainer != null)
        {
            // Prevent duplicates across both lines of containers
            if (!IsDuplicateItemInInventory(containerNames, itemName, OnelineItemContainer) &&
                !IsDuplicateItemInInventory(containerNames2, itemName, SecondlineItemContainer))
            {
                // Check for empty spaces in the first line of containers
                foreach (var containerName in containerNames)
                {
                    var itemContainer = OnelineItemContainer.Q<VisualElement>(containerName);
                    if (itemContainer != null && itemContainer.childCount == 0) // Check if the container is empty
                    {
                        AddItemToContainer(itemContainer, itemName, itemImage);
                        Debug.Log(itemName + " added to " + containerName);
                        return;
                    }
                }

                // Check for empty spaces in the second line of containers
                foreach (var containerName in containerNames2)
                {
                    var itemContainer = SecondlineItemContainer.Q<VisualElement>(containerName);
                    if (itemContainer != null && itemContainer.childCount == 0) // Check if the container is empty
                    {
                        AddItemToContainer(itemContainer, itemName, itemImage);
                        Debug.Log(itemName + " added to " + containerName);
                        return;
                    }
                }
            }
        }
        // If no empty container is found in either line
        Debug.Log("No empty container found for item " + itemName);
    }

    // Helper method to add an item to a container
    private void AddItemToContainer(VisualElement itemContainer, string itemName, Texture2D itemImage)
    {
        // Check if the item is already present in the container
        foreach (var child in itemContainer.Children())
        {
            if (child is VisualElement childElement)
            {
                var existingLabel = childElement.Q<Label>();
                if (existingLabel != null && existingLabel.text == itemName)
                {
                    Debug.Log(itemName + " is already in the inventory. Skipping.");
                    return; // Exit the method if duplicate found
                }
            }
        }
        // Create a new VisualElement for the item
        VisualElement newItem = new VisualElement();
        newItem.AddToClassList("inventory-item");

        // Create an Image element to display the item
        var itemImageElement = new Image();
        itemImageElement.image = itemImage;

        // Create a Label to display the item name
        var itemLabel = new Label(itemName);
        itemLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        itemLabel.style.alignSelf = Align.Center;
        itemLabel.style.flexGrow = 1;
        itemLabel.style.unityFontStyleAndWeight = FontStyle.Bold; // Make the font bold
        itemLabel.style.fontSize = 16;                           // Adjust font size

        // Add the image and label to the new item
        newItem.Add(itemImageElement);
        newItem.Add(itemLabel);

        // Add the new item to the empty container
        itemContainer.Add(newItem);
    }

    private bool IsDuplicateItemInInventory(string[] containerNames, string itemName, VisualElement rootContainer)
    {
        foreach (var containerName in containerNames)
        {
            var itemContainer = rootContainer.Q<VisualElement>(containerName);
            if (itemContainer != null)
            {
                // Loop through each child element of the container
                foreach (var child in itemContainer.Children())
                {
                    if (child is VisualElement childElement)
                    {
                        var existingLabel = childElement.Q<Label>(); // Get the Label that contains the item name
                        if (existingLabel != null && existingLabel.text == itemName)
                        {
                            return true; // Duplicate found
                        }
                    }
                }
            }
        }
        return false; // No duplicates found
    }

    // get player position 
    public Vector3 GetPlayerPosition()
    {
        if (PlayerManager.Instance != null)
        {
            Vector3 playerPos = PlayerManager.Instance.GetPlayerPosition();
            Debug.Log("Player Position from another scene: " + playerPos);
            return playerPos;
        }
        else
        {
            Debug.LogError("PlayerManager instance is null!");
            //return 999, 999, 999; // Return a default value if PlayerManager is not assigned
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
            Debug.Log("Ground Dimension from another scene: " + groundDimensions);
            return groundDimensions;
        }
        else
        {
            Debug.LogError("PlayerManager instance is null!");
            //return 999, 999, 999; // Return a default value if PlayerManager is not assigned
            return Vector3.zero;
        }
    }

    private void UpdateMapState()
    {
        
        // Get player's position using the new function
        Vector3 playerPos = GetPlayerPosition();

        // Get the ground dimensions using the new function
        Vector3 groundDimension = GetGroundDimension();
        Debug.Log("Ground Dimension from another scene: " + groundDimension);
        Debug.Log("Player Position from another scene: " + playerPos);

        // get dimension multiplier
        var multiplyerX = groundDimension.x / 100;
        var multiplyerY = groundDimension.z / 100;

        float x = (playerPos.x + (groundDimension.x / 2)) / multiplyerX;
        float y = (playerPos.z + (groundDimension.z / 2)) / multiplyerY;

        _playerRepresentation.style.left = new Length(x, LengthUnit.Percent);
        _playerRepresentation.style.top = new Length(y, LengthUnit.Percent);
        float playerRotationY = PlayerManager.Instance.transform.rotation.eulerAngles.y;
        _playerRepresentation.style.rotate = new Rotate(new Angle(playerRotationY, AngleUnit.Degree));

        // log this to the console
        Debug.Log("Player icon position: " + x + ", " + y);

    }
}
