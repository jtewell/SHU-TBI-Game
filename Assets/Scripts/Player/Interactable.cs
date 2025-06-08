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

    [Header( "Outline Effect" )]
    public Material hoverGlowShaderMaterial;
    public float inRangeThickness = 1.1f;
    [FormerlySerializedAs("hoverThickness")] public float outOfRangeThickness = 1.1f;
    public bool glowOutOfRange = true;
    [ColorUsage(true, true)] public Color inRangeOutline = Color.cyan;
    [FormerlySerializedAs("colorOutline")] [ColorUsage(true, true)] public Color outOfRangeOutline = Color.yellow;

    [Header("Interaction Settings")]
    [SerializeField] private bool canInteractAnywhere = false;

    //Private variables
    private GameObject hoverParent;
    private List<Renderer> hoverRenderers = new List<Renderer>();
    private bool near = false;
    private bool _canInteract = true;

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
    private void Start()
    {
        if(!hoverGlowShaderMaterial) hoverGlowShaderMaterial = Resources.Load<Material>("Materials/GlowShader");
        var collider = GetComponent<SphereCollider>();
        if (!collider.isTrigger)
        {
            Debug.LogWarning("Interactable must have a sphere collider with isTrigger set to true. Fixing it for you.");
            collider.isTrigger = true;
        }
        //hoverParent = Instantiate(new GameObject("HoverParent"), transform.position, transform.rotation);
        AddRenderersFromObject(gameObject);
        foreach (var children in gameObject.GetComponentsInChildren<Renderer>())
        {
            AddRenderersFromObject(children.gameObject);
        }
        outOfRange();

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

    private void DisableInteraction ()
    {
        _canInteract = false;
    }

    private void EnableInteraction ()
    {
        _canInteract = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;

        near = true;

        foreach (var hoverRenderer in hoverRenderers)
        {
            hoverRenderer.enabled = true;
            hoverRenderer.material.SetFloat("_Thickness", inRangeThickness);
            hoverRenderer.material.SetColor("_OutlineColor", inRangeOutline);
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        near = false;
        outOfRange();
    }

    private void outOfRange()
    {
        if (glowOutOfRange)
        {
            foreach (var hoverRenderer in hoverRenderers)
            {
                hoverRenderer.enabled = true;
                hoverRenderer.material.SetFloat("_Thickness", outOfRangeThickness);
                hoverRenderer.material.SetColor("_OutlineColor", outOfRangeOutline);
            }
        }
        else
        {
            foreach (var hoverRenderer in hoverRenderers)
            {
                hoverRenderer.enabled = false;
            }
        }
    }


    private void AddRenderersFromObject(GameObject obj)
    {
        if (!obj.TryGetComponent<Renderer>(out Renderer originalRenderer)) return;

        // Create a child GameObject to hold the outline mesh
        GameObject outlineObject = new GameObject($"{obj.name}_Outline");
        outlineObject.transform.SetParent(obj.transform, false); // Keeps local position/rotation/scale

        // Copy mesh and position from the original
        MeshFilter originalMesh = obj.GetComponent<MeshFilter>();
        MeshRenderer originalMeshRenderer = obj.GetComponent<MeshRenderer>();

        if (originalMesh != null && originalMeshRenderer != null)
        {
            outlineObject.AddComponent<MeshFilter>().sharedMesh = originalMesh.sharedMesh;
            var rend = outlineObject.AddComponent<MeshRenderer>();
            rend.material = hoverGlowShaderMaterial;
            rend.material.SetFloat("_Thickness", inRangeThickness);
            rend.material.SetColor("_OutlineColor", inRangeOutline);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            rend.enabled = false;

            this.hoverRenderers.Add(rend);
        }
    }


    private void OnMouseDown()
    {
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
        RaycastHit[] hitObjects = Physics.RaycastAll(ray);

        //Check to see if the ray cast hit the main collider
        bool hitSuccessful = hitObjects.Any(hitObject => hitObject.collider == mainCollider);

        //If it did not, then return false
        if (hitSuccessful == false) return false;

        //If it did, return true
        return true;
    }

    private bool IsObjectUnderUI ()
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
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
