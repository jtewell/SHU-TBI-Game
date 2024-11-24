using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "SHU/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public string questId;
    public string questDisplayName;

    [SerializeField]
    public BaseStep[] steps;
}
