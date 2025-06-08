using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable] public class OnTodoListOpenedEvent : UnityEvent<QuestUIControllerUGUI> { }
[System.Serializable] public class OnTodoListClosedEvent : UnityEvent<QuestUIControllerUGUI> { }

public class QuestUIControllerUGUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TMP_Text questTitle;
    public GameObject stepList;
    public GameObject contentHolder;
    public GameObject itemTemplate;
    public Sprite checkmarkOn;
    public Sprite checkmarkOff;
    public OnTodoListOpenedEvent onTodoListOpenedEvent;
    public OnTodoListClosedEvent onTodoListClosedEvent;

    private void Start()
    {
        RenderQuest();
        if (QuestManager.Instance == null)
        {
            Debug.LogError("Quest Manager is null. Cannot subscribe for future rerenders");
            return;
        }
        QuestManager.onStepChecked.AddListener(RenderQuest);
        stepList.SetActive(false);
        
    }

    public void RenderQuest()
    {
        //Debug.Log("Quest UI Controller at " + gameObject.name + " is rendering quest ");
        if (QuestManager.Instance == null) return;
        foreach (var child in contentHolder.transform)
        {
            if(child is RectTransform rectTransform) Destroy(rectTransform.gameObject);
        }
        

        foreach (var step in QuestManager.Instance.CurrentQuest.steps)
        {
            GameObject stepItem = Instantiate(itemTemplate, contentHolder.transform);
            Image checkmark = stepItem.GetComponentInChildren<Image>();
            TMP_Text stepText = stepItem.GetComponentInChildren<TMP_Text>();
            stepText.text = step.InstructionText;
            //Debug.Log("Adding step " + step.InstructionText + " (" + step.IsCompleted + ")");
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
        onTodoListOpenedEvent?.Invoke(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        stepList.SetActive(false);
        onTodoListClosedEvent?.Invoke(this);
    }
}
