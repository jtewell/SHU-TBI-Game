using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject InventoryUI;
    public GameObject[] slots;

    public void OnInventoryButtonClicked()
    {
        InventoryUI.SetActive(true);
    }
    public void OnCloseButtonClicked()
    {
        InventoryUI.SetActive(false);
    }

    private void OnEnable()
    {
        //Reset all slots
        for (int i = 0; i < 8; i++)
        {
            //Set the slot image sprite to nothing
            slots[i].GetComponent<Image>().sprite = null;
        }

        //go through the whole inventory and draw each item to its corresponding slot
        for (int i = 0; i < InventoryManager.Instance.InventoryList.Count; i++)
        {
            slots[i].GetComponent<Image>().sprite = InventoryManager.Instance.InventoryList[i].inventorySprite;
        }
    }
}
