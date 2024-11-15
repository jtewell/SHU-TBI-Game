using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManagment : MonoBehaviour
{
    // Reference to the UI Document that holds the inventory UI elements 
    private UIDocument uiInventory;
    public VisualTreeAsset InventoryItems;
    private VisualElement inventoryContainer;



    // Start is called before the first frame update 
    private void OnEnable()
    {
        uiInventory = GetComponent<UIDocument>();

    }
     public void PopulateInventoryUI()
    {
        if (uiInventory == null )
        {
            Debug.LogError("UI Inventory ");
            return;
        }
        if (InventoryItems == null)
            {
                Debug.LogError("InventoryItems is not assigned");
                return;
            }
        TemplateContainer itemButtonContainer = InventoryItems.Instantiate();
        var itemContainer = uiInventory.rootVisualElement.Q("itemContainer");

        if (itemContainer == null)
        {
            Debug.LogError("Item container not found in the UI");
            return;
        }

        itemContainer.Clear(); // Clear existing items
        itemContainer.Add(itemButtonContainer);

        // Set the size of the container (make it larger, for example, 80% of the screen size)
        itemContainer.style.width = Length.Percent(80);  // Adjust the percentage as needed
        itemContainer.style.height = Length.Percent(80);  // Adjust the percentage as needed

        // Set the parent container to use Flexbox layout (to help center the child container)
        itemContainer.style.position = Position.Absolute;
        itemContainer.style.left = Length.Percent(50);  // Position horizontally at the center
        itemContainer.style.top = Length.Percent(50);   // Position vertically at the center
        itemContainer.style.transformOrigin = new TransformOrigin(0.5f, 0.5f, 0);

        // Ensure proper centering by applying flexbox to the parent of itemContainer
        itemContainer.style.alignSelf = Align.Center;  // Align the container at the center of the parent
        //itemContainer.style.justifySelf = Justify.Center;  // Align it horizontally in the middle of the parent

        // You can adjust the margins to fine-tune the placement if needed (though flexbox should handle it)


        // Adjust the position to ensure it's truly centered considering its size
        itemContainer.style.marginLeft = Length.Percent(-40); // 50% minus half of the width (50% - 40%)
        itemContainer.style.marginTop = Length.Percent(-50);  // 50% minus half of the height (50% - 40%)

    }


}
   
