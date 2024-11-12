using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class rightsideMenu : MonoBehaviour
{

    private VisualElement  overlayMenu, rightMenuContainer, inventoryContainer;
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
        mapButton = root.Q<Button>("mapButton");
        inventoryButton = root.Q<Button>("inventoryButton");
        closeButton = root.Q<Button>("closeButton");

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

        // Add a click event listener to the close button
        if (closeButton != null)
        {
            closeButton.clicked += () =>
            {
                // Hide the overlay menu
                inventoryContainer.visible = false;
            };
        }
        else
        {
            Debug.LogError("Close button not found");
        }

    }
    private void OnInventoryAccessClick()
    {
        // Toggle the inventory container visibility
        //Debug.Log("Inventory Button clicked");
        inventoryContainer.visible = !inventoryContainer.visible;

    }

    // Update is called once per frame
    void Update()
    {
    }

   
}
