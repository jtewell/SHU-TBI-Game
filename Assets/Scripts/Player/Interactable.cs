using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yarn.Unity;
using System.Linq;

[RequireComponent( typeof(SphereCollider) )]
public class Interactable : MonoBehaviour
{
    //Public or serialized variables
    [Header("Outline Effect")]
    public bool outlineEnabled = false;

    [Header("Interaction Settings")]
    [Tooltip("true ignores proximity to player for interaction (like table items)")]
    [SerializeField] private bool canInteractAnywhere = false;
    [Tooltip("true prevents item from getting disabled during dialog (like the table interaction).")]
    [SerializeField] private bool dontDisableDuringDialog = false;

    //Private variables
    private bool near = false;
    public bool _canInteract = true;

    [Serializable]
    /// <summary>
    /// Function definition for a button click event.
    /// </summary>
    public class InteractionEvent : UnityEvent {}

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private Interactable.InteractionEvent m_InteractAction = new Interactable.InteractionEvent();

    private static readonly List<RaycastResult> _raycastHits = new();

    // Start is called before the first frame update
    private void Start()
    {
        //if(!hoverGlowShaderMaterial) hoverGlowShaderMaterial = Resources.Load<Material>("Materials/GlowShader");
        var collider = GetComponent<SphereCollider>();
        if (!collider.isTrigger)
        {
            Debug.LogWarning("Interactable must have a sphere collider with isTrigger set to true. Fixing it for you.");
            collider.isTrigger = true;
        }

        //Listen for dialogue events
        DialogueSystem.Instance.DialogueRunner.onDialogueStart.AddListener(DisableInteraction);
        DialogueSystem.Instance.DialogueRunner.onDialogueComplete.AddListener(EnableInteraction);

        //Listen for UI Events
        MapUIManager.onMapOpenedEvent.AddListener(DisableInteraction);
        MapUIManager.onMapClosedEvent.AddListener(EnableInteraction);
        InventoryUIManager.onInventoryOpenedEvent.AddListener(DisableInteraction);
        InventoryUIManager.onInventoryClosedEvent.AddListener(EnableInteraction);
    }

    private void OnEnable()
    {
        if (DialogueSystem.HasInstance)
        {
            DialogueSystem.Instance.DialogueRunner.onDialogueStart.RemoveListener(DisableInteraction);
            DialogueSystem.Instance.DialogueRunner.onDialogueComplete.RemoveListener(EnableInteraction);

            //Listen for dialogue events
            DialogueSystem.Instance.DialogueRunner.onDialogueStart.AddListener(DisableInteraction);
            DialogueSystem.Instance.DialogueRunner.onDialogueComplete.AddListener(EnableInteraction);
        }

        //Stop listening for map events
        MapUIManager.onMapOpenedEvent.AddListener(DisableInteraction);
        MapUIManager.onMapClosedEvent.AddListener(EnableInteraction);

        //Stop listening for inventory events
        InventoryUIManager.onInventoryOpenedEvent.AddListener(DisableInteraction);
        InventoryUIManager.onInventoryClosedEvent.AddListener(EnableInteraction);
    }

    private void OnDisable()
    {
        //Stop Listening for dialogue events
        if (DialogueSystem.HasInstance)
        {
            DialogueSystem.Instance.DialogueRunner.onDialogueStart.RemoveListener(DisableInteraction);
            DialogueSystem.Instance.DialogueRunner.onDialogueComplete.RemoveListener(EnableInteraction);
        }

        //Stop listening for map events
        MapUIManager.onMapOpenedEvent.RemoveListener(DisableInteraction);
        MapUIManager.onMapClosedEvent.RemoveListener(EnableInteraction);

        //Stop listening for inventory events
        InventoryUIManager.onInventoryOpenedEvent.RemoveListener(DisableInteraction);
        InventoryUIManager.onInventoryClosedEvent.RemoveListener(EnableInteraction);
    }

    public void DisableInteraction ()
    {
        if (!dontDisableDuringDialog) _canInteract = false;
    }

    public void EnableInteraction ()
    {
        if (!dontDisableDuringDialog) _canInteract = true;
    }

    public void ForceEnableInteraction ()
    {
        _canInteract = true;
    }

    public void ForceDisableInteraction ()
    {
        _canInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;

        outlineEnabled = true;

        near = true;
    }
    

    private void OnTriggerExit(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        outlineEnabled = false;
        near = false;
    }

    private void OnMouseDown()
    {
        Debug.Log(name);
        //If the player isn't near the object then don't interact with it (unless canInteractAnywhere is true).
        if (near == false && canInteractAnywhere == false) return;

        //Check to see if the player actually clicked on the object's main collider and not the sphere one
        if (ClickedOnInteractableObject() == false) return;

        //If this object is currently marked with don't interact then don't interact with it
        if (_canInteract == false) return;

        //If this component is currently disabled, then don't interact with it
        if (enabled == false) return;

        //If this object overlaps with a UI element, then don't interact with it
        if (IsObjectUnderUI() == true) return;

        //All checks passed, interact!
        Interact();
    }

    private bool ClickedOnInteractableObject ()
    {
        //Get the object's main collider (the one not marked as a trigger)
        Collider mainCollider = GetComponents<Collider>().FirstOrDefault (c => !c.isTrigger);

        if (mainCollider == null)
        {
            Debug.Log("No non-trigger collider on this interactable object found!");
            return false;
        }

        //Cast a ray from the mouse position on the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Get the list of all objects the ray cast hit
        RaycastHit[] hitObjects = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore);

        //Check to see if the ray cast hit the main collider
        bool hitSuccessful = hitObjects.Any(hitObject => hitObject.collider == mainCollider);

        //If it did not, then return false. If it did, return true
        return hitSuccessful;
    }

    private bool IsObjectUnderUI()
    {
        if (EventSystem.current == null) return false;

        // Mouse
        if (Input.mousePresent && EventSystem.current.IsPointerOverGameObject())
            return true;

        // Touch
        for (int i = 0; i < Input.touchCount; i++)
        {
            var t = Input.GetTouch(i);
            if (t.phase == UnityEngine.TouchPhase.Ended || t.phase == UnityEngine.TouchPhase.Canceled) continue;

            if (IsScreenPointOverUI(t.position))
                return true;
        }

        return false;
    }

    private bool IsScreenPointOverUI(Vector2 screenPos)
    {
        _raycastHits.Clear();
        var data = new PointerEventData(EventSystem.current) { position = screenPos };
        EventSystem.current.RaycastAll(data, _raycastHits);
        return _raycastHits.Count > 0;
    }


    // Called by the controller. Should call the defined effect. Flow control is handled by the controller/caller
    void Interact()
    {
    #if UNITY_EDITOR
    Debug.Log($"An Interaction has been requested at {this.gameObject.name}.");    
    #endif
        m_InteractAction?.Invoke();
    }
}
