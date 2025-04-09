using System;
using System.Collections;
using System.Collections.Generic;
using Development.QuestsSystem.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class QuestUIControllerUGUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TMP_Text questTitle;
    public GameObject stepList;
    public GameObject contentHolder;
    public GameObject itemTemplate;
    public Sprite checkmarkOn;
    public Sprite checkmarkOff;

    private void Start()
    {
        RenderQuest();
        if (QuestManager.Instance == null)
        {
            Debug.LogError("Quest Manager is null. Cannot subscribe for future rerenders");
            return;
        }
        QuestManager.Instance.onFinishStep.AddListener(RenderQuest);
        stepList.SetActive(false);
        
    }

    public void RenderQuest()
    {
        Debug.Log("Quest UI Controller at " + gameObject.name + " is rendering quest ");
        if (QuestManager.Instance == null) return;
        foreach (var child in contentHolder.transform)
        {
            if(child is RectTransform rectTransform) Destroy(rectTransform.gameObject);
        }
        

        foreach (var step in QuestManager.Instance.currentQuest.steps)
        {
            GameObject stepItem = Instantiate(itemTemplate, contentHolder.transform);
            Image checkmark = stepItem.GetComponentInChildren<Image>();
            TMP_Text stepText = stepItem.GetComponentInChildren<TMP_Text>();
            stepText.text = step.InstructionText;
            Debug.Log("Adding step " + step.InstructionText + " (" + step.IsCompleted + ")");
            checkmark.sprite = step.IsCompleted ? checkmarkOn : checkmarkOff;
            stepItem.SetActive(true);
            

        }
        
    }
    
    

    public void CheckTodoList ()
    {
        RenderQuest();
    }

    public void OnSelect(BaseEventData eventData)
    {
        
    }

    public void ButtonClicked()
    {
        stepList.SetActive(!stepList.activeInHierarchy);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        stepList.SetActive(false);
    }
}
