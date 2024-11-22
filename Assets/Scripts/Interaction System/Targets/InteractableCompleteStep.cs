using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCompleteStep : MonoBehaviour
{
    public QuestController questController;


    public void FinishStep(string step)
    {
        questController.FinishStep(step);
    }
}
