using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GenderMeshTextures
{
    public Mesh bodyMesh;
    public Texture[] skinTextures = new Texture[4];  // 4 for White, Pale, LightBrown, DarkBrown
}

public class AvatarTextures : MonoBehaviour
{
    // In the Inspector, set up 3 elements in genderOptions (one for each gender).
    // Each element has a "bodyMesh" field and a 4-element "skinTextures" array.
    public GenderMeshTextures[] genderOptions;
    // e.g. genderOptions[0] = male, [1] = female, [2] = nonbinary.

    public Texture[] faceTextures; // Index: [0]=White, [1]=Pale, [2]=LightBrown, [3]=DarkBrown

    public GameObject[] hairStyles;
    public Material[] hairMaterials;

    public SkinnedMeshRenderer avatarSkinnedMesh;
    public Material faceMaterial;
    public Material bodyMaterial;

    public AvatarData avatarData;
    public GameObject hairPrefab;

    

    private void Start()
    {
        SwitchAvatarModelAndTextures();
    }

    private void SwitchAvatarModelAndTextures()
    {
        // Get array indices from enums
        int genderIndex = GetGenderIndex(avatarData.gender);
        int colorIndex = GetSkinColorIndex(avatarData.skinColor);

        //Create new material instances
        Material bodyMaterialInstance = new Material(bodyMaterial);
        Material faceMaterialInstance = new Material(faceMaterial);
        Material hairMaterialInstance = new Material(hairMaterials[colorIndex]);

        //Assign the material instances to the renderer
        Material[] skinnedMeshMaterials = avatarSkinnedMesh.materials;
        skinnedMeshMaterials[0] = bodyMaterialInstance;
        skinnedMeshMaterials[1] = faceMaterialInstance;
        avatarSkinnedMesh.materials = skinnedMeshMaterials;


        // Assign the correct mesh based on gender
        avatarSkinnedMesh.sharedMesh = genderOptions[genderIndex].bodyMesh;

        // Assign body texture based on gender + color
        bodyMaterialInstance.mainTexture = genderOptions[genderIndex].skinTextures[colorIndex];

        // Assign face texture based on color
        faceMaterialInstance.mainTexture = faceTextures[colorIndex];

        //Assign hairstyle based on gender
        hairPrefab = hairStyles[genderIndex];

        //Attach the hair to the body
        Transform bone = avatarSkinnedMesh.bones.FirstOrDefault(b => b != null && b.name == "DEF-Hair");
        if (bone != null)
        {
            GameObject hair = Instantiate(hairPrefab);
            hair.transform.SetParent(bone, false);
            hair.transform.localPosition = Vector3.zero;
            hair.transform.localScale = Vector3.one;
            hair.transform.localRotation = Quaternion.identity;

            Material[] hairMeshMaterials = hair.GetComponent<MeshRenderer>().sharedMaterials;
            hairMeshMaterials[0] = hairMaterialInstance;
            hair.GetComponent<MeshRenderer>().sharedMaterials = hairMeshMaterials;

        }
    }

    private int GetGenderIndex(AvatarData.Gender gender)
    {
        switch (gender)
        {
            case AvatarData.Gender.Male: 
                return 0;
            case AvatarData.Gender.Female: 
                return 1;
            case AvatarData.Gender.NonBinary: 
                return 2;
        }
        // Fallback
        return 0;
    }

    private int GetSkinColorIndex(AvatarData.SkinColor color)
    {
        switch (color)
        {
            case AvatarData.SkinColor.White: 
                return 0;
            case AvatarData.SkinColor.Pale: 
                return 1;
            case AvatarData.SkinColor.LightBrown: 
                return 2;
            case AvatarData.SkinColor.DarkBrown: 
                return 3;
        }
        // Fallback
        return 0;
    }
}
