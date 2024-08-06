using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickupItem : MonoBehaviour
{
    // public Item item;
    // Still need to implement connection with InventoryManager in order to store the item in

    public void onPickup()
    {

        Destroy(gameObject);
    }
}
