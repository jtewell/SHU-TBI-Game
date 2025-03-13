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
    
    private GameObject hoverParent;
    private List<Renderer> hoverRenderers = new List<Renderer>();
    
    public string DisplayText = "Press E to Use";
    private bool m_HasInteractedInPreviousFrame = false;
    
    
    
    [Header( "Outline Effect" )]
    public Material hoverGlowShaderMaterial;
    public float inRangeThickness = 1.1f;
    [FormerlySerializedAs("hoverThickness")] public float outOfRangeThickness = 1.1f;
    public bool glowOutOfRange = true;
    [ColorUsage(true, true)] public Color inRangeOutline = Color.cyan;
    [FormerlySerializedAs("colorOutline")] [ColorUsage(true, true)] public Color outOfRangeOutline = Color.yellow;
    
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
        var collider = GetComponent<BoxCollider>();
        if (!collider.isTrigger)
        {
            Debug.LogWarning("Interactable must have a BoxCollider with isTrigger set to true. Fixing it for you.");
            collider.isTrigger = true;
        }
        //hoverParent = Instantiate(new GameObject("HoverParent"), transform.position, transform.rotation);
        AddRenderersFromObject(gameObject);
        foreach (var children in gameObject.GetComponentsInChildren<Renderer>())
        {
            AddRenderersFromObject(children.gameObject);
        }
        outOfRange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        
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
        Renderer originalRenderer;
        if(!obj.TryGetComponent<Renderer>(out originalRenderer)) return;
        
        GameObject outlineObject = Instantiate(obj, obj.transform.position, obj.transform.rotation, obj.transform);
        // FPP Problems 
        outlineObject.transform.localPosition = Vector3.one;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = new Vector3(1, 1, 1);
        var rend = outlineObject.GetComponent<Renderer>();
        rend.material = hoverGlowShaderMaterial;
        rend.material.SetFloat("_Thickness", inRangeThickness);
        rend.material.SetColor("_OutlineColor", inRangeOutline);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rend.enabled = false;
        // Disable Components on the clone.
        foreach (var component in outlineObject.GetComponentsInChildren<Behaviour>())
        {
            if (component.GetType() != typeof(MeshRenderer))
            {
                component.enabled = false;
            }
        }
        outlineObject.GetComponent<Collider>().enabled = false;
        //outlineObject.GetComponent<OutlineEffectOnHover>().enabled = false;
        this.hoverRenderers.Add(rend);
    }

    private void OnMouseDown()
    {
        Interact();
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
