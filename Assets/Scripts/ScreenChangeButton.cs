using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ScreenChangeButton : MonoBehaviour
{
    // Declare the buttons
    private Button AcceptButton;
    private Button DeclineButton;
    private VisualElement ThankYouMessage_Container;
    private VisualElement consentForm_ScrollView;
    private VisualElement BottomLayerForm;
    private Label ThankYouMessage;
    private Button ViewButton;
    private Button ExitButton;


    // Start is called before the first frame update
    void Start()
    {
        // Access the UI Document and retrieve the root visual element
        var root = GetComponent<UIDocument>().rootVisualElement;

        //find visual element
        consentForm_ScrollView = root.Q<VisualElement>("consentForm_ScrollView");
        ThankYouMessage_Container = root.Q<VisualElement>("ThankYouMessage_Container");
        BottomLayerForm = root.Q<VisualElement>("BottomLayerForm");


        // Find the button 
        AcceptButton = root.Q<Button>("AcceptButton");
        DeclineButton = root.Q<Button>("DeclineButton");
        ViewButton = root.Q<Button>("ViewButton");
        ExitButton = root.Q<Button>("ExitButton");

        //Find the Label
        ThankYouMessage = root.Q<Label>("ThankYouMessage");

        //check if visual element is found
        if (ThankYouMessage_Container != null)
        {
            ThankYouMessage_Container.style.display = DisplayStyle.None;
        }
        else
        {
            Debug.Log("ThankYouMessage_Container not found");
        }

        if (ViewButton != null)
        {
            ViewButton.style.display = DisplayStyle.None;
            ViewButton.clicked += () =>
            {
                Debug.Log("ViewButton clicked");

                // Show the consentForm_ScrollView and hide the ThankYouMessage_Container
                if (consentForm_ScrollView != null)
                {
                    consentForm_ScrollView.style.display = DisplayStyle.Flex;
                    AcceptButton.style.display = DisplayStyle.Flex;
                    DeclineButton.style.display = DisplayStyle.Flex;
                    ViewButton.style.display = DisplayStyle.None;
                    ExitButton.style.display = DisplayStyle.None;
                }

                if (ThankYouMessage_Container != null)
                {
                    ThankYouMessage_Container.style.display = DisplayStyle.None;
                }
            };
        }
        else
        {
            Debug.LogWarning("ViewButton not found");
        }
        if (ExitButton != null)
        {
            ExitButton.style.display = DisplayStyle.None;
        }
        else
        {
            Debug.Log("ExitButton not found");
        }

        // Add a click event to the "AcceptButton" to load the next scene
        if (AcceptButton != null)
        {
            AcceptButton.clicked += () =>
            {
                Debug.Log("AcceptButton Found");
                // Load the next scene
                SceneManager.LoadScene("ScreeningQuestions");
            };
        }
        else
        {
            Debug.Log("AcceptButton not found");
        }
        if (DeclineButton != null)
        {
            DeclineButton.clicked += () =>
            {
                Debug.Log("DeclineButton Found");
                //show a message box to Thank them for their time

                // Display the Thank You message and hide the consent form
                if (ThankYouMessage_Container != null)
                {
                    Debug.Log("I'm inside thank you container");
                    ThankYouMessage_Container.style.display = DisplayStyle.Flex;
                    // Ensure all children of ThankYouMessage_Container are visible
                    foreach (var child in ThankYouMessage_Container.Children())
                    {
                        child.style.display = DisplayStyle.Flex;
                        
                    }
                }

                if (consentForm_ScrollView != null)
                {
                    consentForm_ScrollView.style.display = DisplayStyle.None;
                }
                // Hide the Accept and Decline buttons inside BottomLayerForm
                if (BottomLayerForm != null)
                {
                    Debug.Log("Hiding Accept and Decline buttons, showing View and Exit buttons");

                    if (AcceptButton != null)
                    {
                        AcceptButton.style.display = DisplayStyle.None;
                    }

                    if (DeclineButton != null)
                    {
                        DeclineButton.style.display = DisplayStyle.None;
                    }

                    if (ViewButton != null)
                    {
                        ViewButton.style.display = DisplayStyle.Flex;
                    }

                    if (ExitButton != null)
                    {
                        ExitButton.style.display = DisplayStyle.Flex;
                    }
                }

            };
        }
        else
        {
            Debug.Log("DeclineButton not found");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
