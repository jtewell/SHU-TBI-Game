using UnityEngine;
using UnityEngine.UIElements;

public class InventoryInputHandler : MonoBehaviour
{
    // Reference to the Inventory Management script
    public InventoryManagment inventoryManagment;
    private UIDocument uiInventory;
    private VisualElement inventoryContainer, OneitemHeadContainer;


    // Item data for image and name (you can expand this for more items)
    public Sprite keySprite;
    public Sprite walletSprite;

    private void Start()
    {
        uiInventory = GetComponent<UIDocument>();

        if (uiInventory == null)
        {
            Debug.LogError("UI Inventory not found.");
            return;
        }
        if (inventoryManagment == null)
        {
            Debug.LogError("Inventory Management script not assigned.");
            return;
        }
        if (OneitemHeadContainer == null)
        {
            Debug.LogError("OneitemHeadContainer not found.");
        }
        //OneitemHeadContainer = uiInventory.rootVisualElement.Q("OneitemHeadContainer");
    }
    private void Update()
    {
        // Check for key presses and add corresponding items
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddItemToInventory("Key", keySprite);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AddItemToInventory("Wallet", walletSprite);
        }
    }

    // Method to add item to the inventory container
    private void AddItemToInventory(string itemName, Sprite itemImage)
    {
        if (inventoryManagment == null)
        {
            Debug.LogError("Inventory Management script not assigned.");
            return;
        }

        // Get the itemContainer from the Inventory Management script
        var itemContainer = inventoryManagment.GetComponent<UIDocument>().rootVisualElement.Q("OneitemHeadContainer");

        if (itemContainer == null)
        {
            Debug.LogError("Item container not found.");
            return;
        }
        //use existing container for the item
        
        

        // Create a new VisualElement for the item
        VisualElement itemElement = new VisualElement();
        itemElement.style.flexDirection = FlexDirection.Row;
        itemElement.style.marginBottom = 5;

        // Create and set the image element
        Image imageElement = new Image();
        imageElement.sprite = itemImage;
        imageElement.style.width = 50;
        imageElement.style.height = 50;

        // Create and set the label element
        Label nameLabel = new Label(itemName);
        nameLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        nameLabel.style.marginLeft = 10;

        // Add the image and label to the item element
        itemElement.Add(imageElement);
        itemElement.Add(nameLabel);

        // Add the item element to the item container
        itemContainer.Add(itemElement);

        Debug.Log($"{itemName} added to inventory.");
    }
}
