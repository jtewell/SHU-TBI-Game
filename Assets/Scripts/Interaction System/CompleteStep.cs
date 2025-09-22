using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteStep : MonoBehaviour
{
    public void FinishStep(string step)
    {
        QuestManager.Instance.FinishStep(step);
    }
}
