using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Narrative Data", menuName = "Measurements/Narrative Data", order = 3)]
public class NarrativeData : DataBaseScriptableObject
{
    [Header("Session Metrics")]
    //The total distance the player travels within the Narrative/Scenario
    public DataPair distanceTraveled;

    //The total time the player takes to complete the Narrative/Scenario
    public DataPair timeTaken;

    //The number of times the player stops for longer than 5 seconds
    public DataPair numberTimesStopped;

    [Header("Menu Opens")]
    // The number of times the player opens each menu type
    public DataPair numberTimesMapOpened;
    public DataPair numberTimesInventoryOpened;
    public DataPair numberTimesToDoOpened;

    [Header("Time in Menus")]
    //The amount of time the player spends in each menu type (total)
    public DataPair timeMapOpened;
    public DataPair timeInventoryOpened;
    public DataPair timeToDoListOpened;

    [Header("Interactions Log")]
    //The number of times the player interacts with each entity type
    public DataPair numberDoorsInteracted;
    public DataPair numberItemsInteracted;
    public DataPair numberNPCSInteracted;
}