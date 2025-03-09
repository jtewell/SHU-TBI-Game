using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public SkinnedMeshRenderer avatarSkinnedMesh;
    public Material faceMaterial;
    public Material bodyMaterial;

    public AvatarData avatarData;

    private void Start()
    {
        SwitchAvatarModelAndTextures();
    }

    private void SwitchAvatarModelAndTextures()
    {
        // Get array indices from enums
        int genderIndex = GetGenderIndex(avatarData.gender);
        int colorIndex = GetSkinColorIndex(avatarData.skinColor);

        // Assign the correct mesh based on gender
        avatarSkinnedMesh.sharedMesh = genderOptions[genderIndex].bodyMesh;

        // Assign body texture based on gender + color
        bodyMaterial.mainTexture = genderOptions[genderIndex].skinTextures[colorIndex];

        // Assign face texture based on color
        faceMaterial.mainTexture = faceTextures[colorIndex];
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
