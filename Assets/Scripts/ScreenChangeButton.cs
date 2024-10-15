using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ScreenChangeButton : MonoBehaviour
{
    // UI elements
    private Button AcceptButton, DeclineButton, ViewButton, ExitButton;
    private VisualElement ThankYouMessage_Container, consentForm_ScrollView, BottomLayerForm;
    private Label ThankYouMessage;
    private Toggle consentToggle;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Initialize UI elements
        consentForm_ScrollView = root.Q<VisualElement>("consentForm_ScrollView");
        ThankYouMessage_Container = root.Q<VisualElement>("ThankYouMessage_Container");
        BottomLayerForm = root.Q<VisualElement>("BottomLayerForm");
        AcceptButton = root.Q<Button>("AcceptButton");
        DeclineButton = root.Q<Button>("DeclineButton");
        ViewButton = root.Q<Button>("ViewButton");
        ExitButton = root.Q<Button>("ExitButton");
        consentToggle = root.Q<Toggle>("consentToggle");
        ThankYouMessage = root.Q<Label>("ThankYouMessage");

        // Hide initially
        ThankYouMessage_Container.style.display = DisplayStyle.None;
        ViewButton.style.display = DisplayStyle.None;
        ExitButton.style.display = DisplayStyle.None;

        // ViewButton click logic
        ViewButton?.RegisterCallback<ClickEvent>(ev =>
        {
            consentForm_ScrollView.style.display = DisplayStyle.Flex;
            AcceptButton.style.display = DisplayStyle.Flex;
            DeclineButton.style.display = DisplayStyle.Flex;
            ViewButton.style.display = DisplayStyle.None;
            ExitButton.style.display = DisplayStyle.None;
            ThankYouMessage_Container.style.display = DisplayStyle.None;
        });
        // Update AcceptButton state when the consentToggle is changed
        consentToggle.RegisterValueChangedCallback(evt =>
        {
            AcceptButton.SetEnabled(evt.newValue);  // Disable/enable based on toggle value
        });

        // AcceptButton click logic
        AcceptButton?.RegisterCallback<ClickEvent>(ev =>
        {
            if (!consentToggle.value)
            {
                //disable accept button
                AcceptButton.SetEnabled(false);
                //AcceptButton.style.display = DisplayStyle.None;
                return;
            }
           
            //enable accept button
            AcceptButton.SetEnabled(true);
            SceneManager.LoadScene("ScreeningQuestions");
            
           
        });

        // DeclineButton click logic
        DeclineButton?.RegisterCallback<ClickEvent>(ev =>
        {
            ThankYouMessage_Container.style.display = DisplayStyle.Flex;
            foreach (var child in ThankYouMessage_Container.Children())
                child.style.display = DisplayStyle.Flex;

            consentForm_ScrollView.style.display = DisplayStyle.None; // Hide the consent form
            AcceptButton.style.display = DisplayStyle.None; // Hide the Accept button
            DeclineButton.style.display = DisplayStyle.None; // Hide the Decline button
            ViewButton.style.display = DisplayStyle.Flex; // Show the View button
            ExitButton.style.display = DisplayStyle.Flex; // Show the Exit button
        });
    }
}
