using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BaseStep
{
    [field: SerializeField]
    public string StepId { get; set; }
    [field: SerializeField]
    public string InstructionText { get; set; }
    [field: SerializeField] //[ReadOnly] - needs Odin asset
    public bool IsCompleted { get; set; }
    
}
