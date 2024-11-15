using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class rightsideMenu : MonoBehaviour
{
    public GameObject inventoryManagerObject; // Reference to the GameObject with InventoryManagement script
    // Reference to the child's script 
    private InventoryManagment inventoryManager;


    private VisualElement  overlayMenu, rightMenuContainer, inventoryContainer, mapContainer, detailed_mapVisualElement, closeMapButtonContainer;
    private Button mapButton, inventoryButton, closeButton;
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


        mapButton = root.Q<Button>("mapButton");
        inventoryButton = root.Q<Button>("inventoryButton");
        closeButton = root.Q<Button>("closeButton");

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

        //Add a click event listener to the close button
        if (closeButton != null)
        {
            closeButton.clicked += () =>
            {
                // Hide the overlay menu
                mapContainer.visible = false;
                detailed_mapVisualElement.visible = false;
                //inventoryContainer.visible = false;
                //inventoryGameObject.SetActive(false); // Hide the inventory UI
            };
        }
        else
{
    Debug.LogError("Close button not found");
}
// Get the InventoryManagement script component
if (inventoryManagerObject == null)
        {
            inventoryManagerObject = transform.Find("Inventory").gameObject; // Replace with the actual name of the child GameObject

        }
        if (inventoryManagerObject != null)
        {
            inventoryManager = inventoryManagerObject.GetComponent<InventoryManagment>();
        }
        else
        {
            Debug.LogError("Inventory Management script not found");
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
            };
        }
        
    }
    private void OnInventoryAccessClick()
    {
        inventoryManager?.PopulateInventoryUI();
        Debug.Log("Inventory UI populated from child GameObject");


    }
    // Update is called once per frame
    void Update()
    {
    }

   
}
