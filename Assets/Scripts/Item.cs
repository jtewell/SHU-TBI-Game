using System.Collections.Generic;
using UnityEngine;

namespace Development
{
    [CreateAssetMenu(fileName = "Quest", menuName = "SHU/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public string id;
        public string displayName;
        
    }
}