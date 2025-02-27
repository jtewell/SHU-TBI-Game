using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ CreateAssetMenu(fileName = "Avatar", menuName = "SHU/Avatar", order = 1)]

public class AvatarData : ScriptableObject
{
    public string Gender;
    public string Name;
    public Texture texture;
}
