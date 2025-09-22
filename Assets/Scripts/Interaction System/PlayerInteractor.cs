using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    

    public GameObject OnInteract;
    public TMP_Text ActionText;

    //private Interactable[] _mCurrentInteractables;
   // private bool m_HasInteractedInPreviousFrame = false;

    private void Start()
    {
        OnInteract.SetActive(false);
    }
}
