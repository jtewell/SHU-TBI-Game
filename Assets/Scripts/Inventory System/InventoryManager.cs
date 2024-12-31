using System.Collections;
using System.Collections.Generic;
using Development;

using UnityEngine;

public class InventoryManager : PersistentMonoSingleton<InventoryManager>
{
    //Class fields
    [SerializeField] private const int INVENTORY_SIZE = 8;
    private List<Item> inventoryList = new List<Item>();

    //Accessors
    public int MaxInventorySize
    {
        get
        {
            return INVENTORY_SIZE;
        }
    }

    public IReadOnlyList <Item> InventoryList
    {
        get
        {
            return inventoryList;
        }
    }


    public void AddItemToInventory(Item item)
    {
        //Check to make sure the inventory isn't over eight items currently
        if (inventoryList.Count == INVENTORY_SIZE)
        {
            Debug.Log("Inventory maxed out- can't carry more!");
            return;
        }

        //Add the item to the inventory
        inventoryList.Add(item);
    }
}
