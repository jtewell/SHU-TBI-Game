using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "SHU/Item", order = 1)]
public class Item : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite inventorySprite;
}