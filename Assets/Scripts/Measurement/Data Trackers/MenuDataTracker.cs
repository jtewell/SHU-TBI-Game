using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDataTracker : MonoBehaviour
{
    private float mapOpenStartTime;
    private float mapOpenEndTime;

    private float inventoryOpenStartTime;
    private float inventoryOpenEndTime;

    private float todoListOpenStartTime;
    private float todoListOpenEndTime;

    // Start is called before the first frame update
    void Start()
    {
        // Start listening for menu events to track
        MapUIManager.onMapOpenedEvent.AddListener(UpdateDataMapOpened);
        MapUIManager.onMapClosedEvent.AddListener(UpdateDataMapClosed);
        InventoryUIManager.onInventoryOpenedEvent.AddListener(UpdateDataInventoryOpened);
        InventoryUIManager.onInventoryClosedEvent.AddListener(UpdateDataInventoryClosed);
    }

    private void OnDisable()
    {
        //Stop listening for menu events to track
        MapUIManager.onMapOpenedEvent.RemoveListener(UpdateDataMapOpened);
        MapUIManager.onMapClosedEvent.RemoveListener(UpdateDataMapClosed);
        InventoryUIManager.onInventoryOpenedEvent.RemoveListener(UpdateDataInventoryOpened);
        InventoryUIManager.onInventoryClosedEvent.RemoveListener(UpdateDataInventoryClosed);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //Callback when there is a map opened event
    public void UpdateDataMapOpened()
    {
        //Increment the amount of times the map has been opened in this narrative
        MeasurementDataManager.Instance.numberTimesMapOpened++;

        //Start logging the map opened time
        mapOpenStartTime = Time.time;
    }

    //Callback when there is a map close event
    public void UpdateDataMapClosed ()
    {
        //Log the current time
        mapOpenEndTime = Time.time;

        //Calculate the amount of time the map was opened
        float mapOpenedTime = mapOpenEndTime - mapOpenStartTime;

        //Add this to the total amount for the narrative
        MeasurementDataManager.Instance.timeMapOpened += mapOpenedTime;
    }

    //Callback when there is a map opened event
    public void UpdateDataInventoryOpened()
    {
        //Increment the amount of times the inventory has been opened in this narrative
        MeasurementDataManager.Instance.numberTimesInventoryOpened++;

        //Start logging the map opened time
        inventoryOpenStartTime = Time.time;
    }

    //Callback when there is a map closed event
    public void UpdateDataInventoryClosed()
    {
        //Log the current time
        inventoryOpenEndTime = Time.time;

        //Calculate the amount of time the map was opened
        float inventoryOpenedTime = inventoryOpenEndTime - inventoryOpenStartTime;

        //Add this to the total amount for the narrative
        MeasurementDataManager.Instance.timeInventoryOpened += inventoryOpenedTime;
    }

    //Callback when there is a todo list opened event
    public void UpdateDataTodoListOpened()
    {
        //Increment the amount of times the todo list has been opened in this narrative
        MeasurementDataManager.Instance.numberTimesToDoListOpened++;

        //Start logging the todo list opened time
        todoListOpenStartTime = Time.time;
    }

    //Callback when there is a todo list closed event
    public void UpdateDataTodoListClosed()
    {
        //Log the current time
        todoListOpenEndTime = Time.time;

        //Calculate the amount of time the map was opened
        float todoListOpenedTime = todoListOpenEndTime - todoListOpenStartTime;

        //Add this to the total amount for the narrative
        MeasurementDataManager.Instance.timeToDoListOpened += todoListOpenedTime;
    }
}
