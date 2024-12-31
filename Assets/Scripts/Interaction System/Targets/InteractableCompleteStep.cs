using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCompleteStep : MonoBehaviour
{
    public void FinishStep(string step)
    {
        QuestManager.Instance.FinishStep(step);
    }
}
