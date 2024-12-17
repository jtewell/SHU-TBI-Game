using System;
using System.Collections;
using System.Collections.Generic;
using Development.QuestsSystem.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIControllerUGUI : MonoBehaviour
{
    public QuestController questController;

    public TMP_Text questTitle;
    public TMP_Dropdown questStepDropdown;
    public Sprite checkmarkOn;
    public Sprite checkmarkOff;

    private void Start()
    {
        RenderQuest();
    }

    public void RenderQuest()
    {
        if (questController == null)
        {
            return;
        }
        questStepDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> stepList = new List<TMP_Dropdown.OptionData>();

        foreach (var step in questController.currentQuest.steps)
        {
            Sprite checkmark;
            if (step.IsCompleted == true)
            {
                checkmark = checkmarkOn;
            }
            else
            {
                checkmark = checkmarkOff;
            }

            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(step.InstructionText, checkmark);
            stepList.Add(option);

        }

        questStepDropdown.AddOptions(stepList);
    }

    public void CheckTodoList ()
    {
        RenderQuest();
    }
}
