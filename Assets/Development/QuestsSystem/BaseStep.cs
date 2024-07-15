using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStep : MonoBehaviour
{
    public string stepId = "";
    public bool isDone = false;
    public string instructionText;
}
