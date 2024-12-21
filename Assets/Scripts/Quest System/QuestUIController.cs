using System;
using System.Collections;
using System.Collections.Generic;
using Development.QuestsSystem.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestUIController : MonoBehaviour
{
    public QuestManager questController;
    public UIDocument uiDocument;
    public VisualTreeAsset stepUITemplate;
    
    private Label questTitle;
    private ListView questSteps;
    

    //TODO: Convince Jordan to Update Unity for DataBinding support
    public void RenderQuest()
    {
        questTitle.text = questController.currentQuest.questDisplayName;
        questSteps.itemsSource = questController.currentQuest.steps;
        questSteps.Rebuild();
    }

    private void OnEnable()
    {
        questTitle = uiDocument.rootVisualElement.Q<Label>("questName");
        questSteps = uiDocument.rootVisualElement.Q<ListView>("stepList");
        questSteps.makeItem = () =>
        {
            var item = stepUITemplate.Instantiate();
            var itemController = new QuestUIItemController();
            item.userData = itemController;
            itemController.SetVisualElement(item);
            return item;
        };
        
        questSteps.bindItem = (item, index) =>
        {
            (item.userData as QuestUIItemController)?.SetStepData(questController.currentQuest.steps[index]);
        };

        questSteps.fixedItemHeight = 100; //TODO: Use Virtualization for Dynamic
        RenderQuest();
        //questController._questUIController = this;
    }

    private void OnDisable()
    {
        
    }
}
