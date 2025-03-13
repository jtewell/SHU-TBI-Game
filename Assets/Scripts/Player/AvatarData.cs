using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ CreateAssetMenu(fileName = "Avatar", menuName = "SHU/Avatar", order = 1)]

public class AvatarData : ScriptableObject
{
    public Gender gender;
    public SkinColor skinColor;

    public enum Gender
    {
        Male,
        Female,
        NonBinary
    }

    public enum SkinColor
    {
        White,
        Pale,
        LightBrown,
        DarkBrown
    }
}
