using System.Linq;
using UnityEngine;

/// <summary>
/// Customizes the look of an NPC avatar by dragging assets into Inspector slots.
///
///   - Body Texture : drag in the body texture  (applied to material slot 0)
///   - Face Texture : drag in the face texture  (applied to material slot 1)
///   - Hair Model   : drag in a hair prefab     (attached to the body's hair bone)
///
/// Empty slots are left alone, so the body model's own textures stay as a fallback.
/// Everything is applied on Awake (before runtime).
/// </summary>
public class NPCAvatar : MonoBehaviour
{
    [Header("Textures (drag in what you want)")]
    [Tooltip("Drag a body texture here. Applied to material slot 0 of the body model.")]
    public Texture bodyTexture;
    [Tooltip("Drag a face texture here. Applied to material slot 1 of the body model.")]
    public Texture faceTexture;

    [Header("Hair (optional - drag in a hair model)")]
    [Tooltip("Drag a hair prefab here and it will be attached to the body's hair bone.")]
    public GameObject hairModel;
    [Tooltip("Name of the bone on the body rig to parent the hair to.")]
    public string hairBoneName = "DEF-Hair";

    [Header("Renderer")]
    [Tooltip("The skinned mesh renderer on the body model. If empty, found automatically among children.")]
    public SkinnedMeshRenderer avatarSkinnedMesh;

    private GameObject hairInstance;
    private Transform hairBone;

    private void Awake()
    {
        ApplyAppearance();
    }

    /// <summary>Applies whatever has been dragged into the slots. Safe to call again after changing assets.</summary>
    public void ApplyAppearance()
    {
        if (avatarSkinnedMesh == null)
            avatarSkinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>(true);

        // Body + face textures
        if (avatarSkinnedMesh != null)
        {
            // Use sharedMaterials: the body is a (nested) prefab object, and Unity
            // forbids Renderer.materials on those. We still create fresh material
            // instances below so the shared base material is never modified.
            Material[] slots = avatarSkinnedMesh.sharedMaterials;
            bool changed = false;

            if (bodyTexture != null && slots.Length > 0)
            {
                slots[0] = CreateMaterialWithTexture(slots[0], bodyTexture);
                changed = true;
            }

            if (faceTexture != null && slots.Length > 1)
            {
                slots[1] = CreateMaterialWithTexture(slots[1], faceTexture);
                changed = true;
            }

            if (changed)
                avatarSkinnedMesh.sharedMaterials = slots;
        }
        else if (bodyTexture != null || faceTexture != null)
        {
            Debug.LogError($"[NPCAvatar] {name}: textures are assigned but no SkinnedMeshRenderer was found. Add a body model (e.g. 'Player_M_Rigged') as a child or assign 'Avatar Skinned Mesh'.", this);
        }

        // Hair: attach the dragged-in model to the body's hair bone
        RemoveHair();
        if (hairModel == null) return;

        Transform bone = avatarSkinnedMesh != null
            ? avatarSkinnedMesh.bones.FirstOrDefault(b => b != null && b.name == hairBoneName)
            : null;

        if (bone == null)
        {
            Debug.LogError($"[NPCAvatar] {name}: bone '{hairBoneName}' not found on the body rig, hair was not attached.", this);
            return;
        }

        // Unity blocks parenting objects into transforms that live inside a prefab,
        // so the hair is a standalone scene object that copies the bone's pose every
        // frame (see LateUpdate) - visually identical to being parented.
        hairBone = bone;
        hairInstance = Instantiate(hairModel);
        hairInstance.transform.SetPositionAndRotation(bone.position, bone.rotation);
        hairInstance.transform.localScale = bone.lossyScale;
    }

    private void LateUpdate()
    {
        if (hairInstance != null && hairBone != null)
        {
            hairInstance.transform.SetPositionAndRotation(hairBone.position, hairBone.rotation);
        }
    }

    /// <summary>Clones the slot's base material (if it has one) and applies the texture.
    /// If the slot is empty, creates a material with a sensible shader instead.</summary>
    private static Material CreateMaterialWithTexture(Material source, Texture texture)
    {
        Material mat = source != null
            ? new Material(source)
            : new Material(GetFallbackShader());
        mat.mainTexture = texture;
        return mat;
    }

    private static Shader GetFallbackShader()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");
        return shader;
    }

    private void RemoveHair()
    {
        if (hairInstance != null)
        {
            Destroy(hairInstance);
            hairInstance = null;
        }
        hairBone = null;
    }

    private void OnDestroy()
    {
        RemoveHair();
    }
}
