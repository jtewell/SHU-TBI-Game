using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManagment : MonoBehaviour
{
    public UIDocument uiDocument; // Assign your UIDocument here
    public string imageFolder = "Assets/Images/InventoryImages"; // Path to your images
    //public VisualTreeAsset itemTemplate; // Optional: Template for items (if using)

    private VisualElement inventoryContainer;

    void Start()
    {
        var rootElement = uiDocument.rootVisualElement;
        // Get the inventory container from the UI Document
        //inventoryContainer = rootElement.rootVisualElement.Q<VisualElement>("inventoryContainer");

        if (inventoryContainer == null)
        {
            Debug.LogError("Inventory container not found!");
        }
    }

    void Update()
    {
        // Example: Add item on pressing the 'I' key
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddItemToInventory("wallet.png", "Wallet");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            AddItemToInventory("key-chain.png", "Keys");
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            AddItemToInventory("umbrella.png", "Umbrella");
        }
    }

    public void AddItemToInventory(string imageName, string itemName)
    {
        if (inventoryContainer == null) return;

        // Create the parent container for the item
        VisualElement itemContainer = new VisualElement();
        itemContainer.style.flexDirection = FlexDirection.Column;
        itemContainer.style.width = 250;
        itemContainer.style.height = 450;
        itemContainer.style.marginLeft = 10;
        itemContainer.style.marginRight = 10;

        // Add the image element
        VisualElement imageElement = new VisualElement();
        imageElement.style.width = 80;
        imageElement.style.height = 80;
        imageElement.style.alignSelf = Align.Center;

        // Load the image dynamically
        string imagePath = $"{imageFolder}/{imageName}";
        var imageTexture = LoadImage(imagePath);
        if (imageTexture != null)
        {
            imageElement.style.backgroundImage = new StyleBackground(imageTexture);
        }
        else
        {
            Debug.LogError($"Image not found at: {imagePath}");
        }

        itemContainer.Add(imageElement);

        // Add the label for the item
        Label itemLabel = new Label(itemName);
        itemLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        itemLabel.style.fontSize = 20;
        itemLabel.style.alignSelf = Align.Center;

        itemContainer.Add(itemLabel);

        // Add the item to the inventory container
        inventoryContainer.Add(itemContainer);
    }

    private Texture2D LoadImage(string path)
    {
        // Load image as Texture2D
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
