using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent( typeof(BoxCollider) )]
public class Interactable : MonoBehaviour
{
    public string DisplayText = "Press E to Use";
    
    [Serializable]
    /// <summary>
    /// Function definition for a button click event.
    /// </summary>
    public class InteractionEvent : UnityEvent {}

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private Interactable.InteractionEvent m_InteractAction = new Interactable.InteractionEvent();
    // Start is called before the first frame update
    

    // Called by the controller. Should call the defined effect. Flow control is handled by the controller/caller
    void Interact()
    {
    #if UNITY_EDITOR
    Debug.Log($"An Interaction has been requested at {this.gameObject.name}.");    
    #endif
        m_InteractAction?.Invoke();
    }
}
