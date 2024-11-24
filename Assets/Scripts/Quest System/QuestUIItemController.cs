using UnityEngine.UIElements;

namespace Development.QuestsSystem.UI
{
    public class QuestUIItemController
    {
        Label stepNameLabel;
        private BaseField<bool> questStatus;
    
        // This function retrieves a reference to the 
        // character name label inside the UI element.
        public void SetVisualElement(VisualElement visualElement)
        {
            stepNameLabel = visualElement.Q<Label>("stepInstructions");
            questStatus = visualElement.Q<Toggle>("questStatus");
            questStatus.pickingMode = PickingMode.Ignore;
            questStatus.Q<VisualElement>().pickingMode = PickingMode.Ignore; // Make it read-only
        }
    
        // This function receives the character whose name this list 
        // element is supposed to display. Since the elements list 
        // in a `ListView` are pooled and reused, it's necessary to 
        // have a `Set` function to change which character's data to display.
        public void SetStepData(BaseStep step)
        {
            stepNameLabel.text = step.InstructionText;
            questStatus.value = step.IsCompleted;
            if (step.IsCompleted)
            {
                stepNameLabel.AddToClassList("done");
            }
            else
            {
                stepNameLabel.RemoveFromClassList("done");
            }
        }
    }
}