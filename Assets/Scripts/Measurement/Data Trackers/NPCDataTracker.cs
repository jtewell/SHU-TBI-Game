using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDataTracker : MonoBehaviour
{
    private void OnEnable()
    {
        InteractableNPC.onNPCInteractEvent.AddListener(UpdateDataNPCTalk);
    }

    private void OnDisable()
    {
        InteractableNPC.onNPCInteractEvent.RemoveListener(UpdateDataNPCTalk);
    }

    public void UpdateDataNPCTalk (string name)
    {
        MeasurementDataManager.Instance.peopleInteracted += name + " , ";
    }
}
