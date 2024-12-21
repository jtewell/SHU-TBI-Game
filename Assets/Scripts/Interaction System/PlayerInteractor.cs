using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float radius = 10f;
    public bool DistanceDebug = false;


    public GameObject OnInteract;
    public TMP_Text ActionText;

    private Interactable _mCurrentInteractable;
    private bool m_HasInteractedInPreviousFrame = false;

    // Update is called once per frame
    void Update()
    {
        bool tickHasObj = false;
        // Detection
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius); // PERF: This could use a NonAlloc method... But no need as of now I guess
        Interactable detectedInteractable;
        foreach (var foundCollider in objectsInRange)
        {
            if (foundCollider.gameObject.TryGetComponent<Interactable>(out detectedInteractable))
            {
                ActionText.text = detectedInteractable.DisplayText;
                tickHasObj = true;
                _mCurrentInteractable = detectedInteractable;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !m_HasInteractedInPreviousFrame)
        {
            m_HasInteractedInPreviousFrame = true;
            _mCurrentInteractable.BroadcastMessage("Interact");

        }
        else
        {
            m_HasInteractedInPreviousFrame = false;
        }
        OnInteract.SetActive(tickHasObj);
    }

    private void OnDrawGizmos()
    {
        if(DistanceDebug) Gizmos.DrawWireSphere(transform.position, radius);
    }
}
