using System.Collections;
using System.Collections.Generic;
using Development;

using UnityEngine;

public class InventoryManager1 : MonoSingleton<InventoryManager1>
{
    private List<Item> Items;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InventoryManager Started");
    }

    // Update is called once per frame
    public int AddItem(Item item)
    {
        Items.Add(item);
        return Items.Count;
    }
}
