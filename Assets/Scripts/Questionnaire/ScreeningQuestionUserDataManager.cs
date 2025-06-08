using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;





public class ScreeningQuestionUserDataManager : MonoBehaviour
{

    // Arrays to hold references to VisualElement and Button components for each question
    private VisualElement[] questions = new VisualElement[30];
    private Button[] previousButtons = new Button[30];
    private Button[] continueButtons = new Button[30];
    private VisualElement ErrorMessageContainer;
    private Button continueButtonNum;

    //keeps track of what question
    private int questionNumber = 1;

    private void OnEnable()
    {
        // Get the UIDocument component attached to the same GameObject
        var uiDocument = GetComponent<UIDocument>();
       
        // Get the root VisualElement of the UI document
        var root = uiDocument.rootVisualElement;
       
        // Initialize the question VisualElements by finding them in the UI hierarchy
        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = root.Q<VisualElement>($"Question{i + 1}");
        }

        // Initialize the previous and continue buttons for each question
        for (int i = 0; i < previousButtons.Length; i++)
        {
            previousButtons[i] = root.Q<Button>($"Q{i + 1}PreviousButton");
            continueButtons[i] = root.Q<Button>($"Q{i + 1}Continue");

        }

        // Add click event listeners to the previous and continue buttons
        for (int i = 0; i < previousButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (previousButtons[i] != null)
                previousButtons[i].clicked += () => OnPreviousButtonClick(index + 1);
            if (continueButtons[i] != null)
            {
                continueButtonNum = continueButtons[i];

                continueButtons[i].clicked += () => OnContinueButtonClick(index + 1);
            }
            // Disable the continue buttons on load (they will be enabled when the user selects an option)
            if (continueButtons[i] != null)
                continueButtons[i].SetEnabled(false); // Disable on load
        }

        //Run after 100 ms (running sooner will accidentally trigger the callback due to the form getting filled)
        root.schedule.Execute(() => { RegisterSelectionEvents(root); }).StartingIn(100);
    }

    //This will loop through each element of the UI Form that can be interacted with and registers OnSelectionMade with them
    private void RegisterSelectionEvents(VisualElement element)
    {
        foreach (var child in element.Children())
        {
            if (child is DropdownField dropdownField)
            {
                dropdownField.RegisterValueChangedCallback(evt => OnSelectionMade());
            }

            if (child is Button button)
            {
                
                button.RegisterCallback<ClickEvent>(evt => OnSelectionMade());
            }

            RegisterSelectionEvents(child);
        }
    }

    //Any time the user performs some selection (i.e. button click) this will be triggered to enable (or not) the continue buttons
    private void OnSelectionMade()
    {
       
        if (questionNumber == 1 && !(string.IsNullOrEmpty(MeasurementDataManager.Instance.birthMonth) || 
            string.IsNullOrEmpty(MeasurementDataManager.Instance.birthDay) || 
            string.IsNullOrEmpty(MeasurementDataManager.Instance.birthYear)))
        {
            continueButtons[0].SetEnabled(true);
        }

        if (questionNumber == 2 && !string.IsNullOrEmpty(MeasurementDataManager.Instance.gender))
        {

            continueButtons[1].SetEnabled(true);
        }

        if (questionNumber > 2 && questionNumber < 30)
        {
            string[] selectedOptions = { null, null,
            MeasurementDataManager.Instance.Q3SelectedOption, MeasurementDataManager.Instance.Q4SelectedOption, 
            MeasurementDataManager.Instance.Q5SelectedOption, MeasurementDataManager.Instance.Q6SelectedOption, 
            MeasurementDataManager.Instance.Q7SelectedOption, MeasurementDataManager.Instance.Q8SelectedOption, 
            MeasurementDataManager.Instance.Q9SelectedOption, "yes", 
            MeasurementDataManager.Instance.Q11SelectedOption, MeasurementDataManager.Instance.Q12SelectedOption, 
            MeasurementDataManager.Instance.Q13SelectedOption, MeasurementDataManager.Instance.Q14SelectedOption, 
            MeasurementDataManager.Instance.Q15SelectedOption, MeasurementDataManager.Instance.Q16SelectedOption, 
            MeasurementDataManager.Instance.Q17SelectedOption, MeasurementDataManager.Instance.Q18SelectedOption,
            MeasurementDataManager.Instance.Q19SelectedOption, MeasurementDataManager.Instance.Q20SelectedOption, 
            MeasurementDataManager.Instance.Q21SelectedOption, MeasurementDataManager.Instance.Q22SelectedOption, 
            MeasurementDataManager.Instance.Q23SelectedOption, MeasurementDataManager.Instance.Q24SelectedOption, 
            MeasurementDataManager.Instance.Q25SelectedOption, MeasurementDataManager.Instance.Q26SelectedOption, 
            MeasurementDataManager.Instance.Q27SelectedOption, MeasurementDataManager.Instance.Q28SelectedOption, 
            MeasurementDataManager.Instance.Q29SelectedOption };

            if (!string.IsNullOrEmpty(selectedOptions[questionNumber - 1]))
            {
                continueButtons[questionNumber - 1].SetEnabled(true);
            }
        }
    }

    // Method to handle the "Previous" button click for a specific question
    private void OnPreviousButtonClick(int questionIndex)
    {
        if (questions[questionIndex - 2] != null && questions[questionIndex - 1] != null)
        {
            
            // Hide the current question and show the previous question
            questions[questionIndex - 2].style.display = DisplayStyle.Flex;
            questions[questionIndex - 1].style.display = DisplayStyle.None;
        }
    }
    private void OnContinueButtonClick(int questionIndex)
    {
        //Update this each time the continue button is selected
        questionNumber = questionIndex + 1;

        bool hasErrors = false;
        //marge three values to form a date

        string Q1SelectedOption = MeasurementDataManager.Instance.birthMonth + "/" + MeasurementDataManager.Instance.birthDay + "/" + MeasurementDataManager.Instance.birthYear;
        string Q2SelectedOption = MeasurementDataManager.Instance.gender;
        string[] selectedOptions = { Q1SelectedOption, Q2SelectedOption,
            MeasurementDataManager.Instance.Q3SelectedOption, MeasurementDataManager.Instance.Q4SelectedOption,
            MeasurementDataManager.Instance.Q5SelectedOption, MeasurementDataManager.Instance.Q6SelectedOption,
            MeasurementDataManager.Instance.Q7SelectedOption, MeasurementDataManager.Instance.Q8SelectedOption, 
            MeasurementDataManager.Instance.Q9SelectedOption, "yes", 
            MeasurementDataManager.Instance.Q11SelectedOption, MeasurementDataManager.Instance.Q12SelectedOption, 
            MeasurementDataManager.Instance.Q13SelectedOption, MeasurementDataManager.Instance.Q14SelectedOption, 
            MeasurementDataManager.Instance.Q15SelectedOption, MeasurementDataManager.Instance.Q16SelectedOption, 
            MeasurementDataManager.Instance.Q17SelectedOption, MeasurementDataManager.Instance.Q18SelectedOption,
            MeasurementDataManager.Instance.Q19SelectedOption, MeasurementDataManager.Instance.Q20SelectedOption, 
            MeasurementDataManager.Instance.Q21SelectedOption, MeasurementDataManager.Instance.Q22SelectedOption, 
            MeasurementDataManager.Instance.Q23SelectedOption, MeasurementDataManager.Instance.Q24SelectedOption, 
            MeasurementDataManager.Instance.Q25SelectedOption, MeasurementDataManager.Instance.Q26SelectedOption, 
            MeasurementDataManager.Instance.Q27SelectedOption, MeasurementDataManager.Instance.Q28SelectedOption, 
            MeasurementDataManager.Instance.Q29SelectedOption };

        //check if the selected option is empty
        if (questionIndex == 1)
        {
            //check if the selected option is empty
            if (string.IsNullOrEmpty(MeasurementDataManager.Instance.birthMonth) || 
                string.IsNullOrEmpty(MeasurementDataManager.Instance.birthDay) || 
                string.IsNullOrEmpty(MeasurementDataManager.Instance.birthYear))
            {
                hasErrors = true;
            }
        }
        
        if (questionIndex >= 2 && questionIndex <= 29)
        {
            if (questionIndex == 10)
            {
                continueButtonNum.SetEnabled(true);
            }
            else
            {        
                if (string.IsNullOrEmpty(selectedOptions[questionIndex - 1]))
                {
                    
                    hasErrors = true;
                }
            }
        }

        else if (questionIndex == 30)
        {
            SubmitDataToSheet();
            SceneManager.LoadScene("Avatar_Selection");
        }

        //Update the continue button state based on whether there are errors and change its color  to dark grey
        continueButtonNum.SetEnabled(!hasErrors);

        if (!hasErrors)
        {
            if (questions[questionIndex - 1] != null)
                questions[questionIndex - 1].style.display = DisplayStyle.None; // Hide the current question

            if (questionIndex < questions.Length && questions[questionIndex] != null)
                questions[questionIndex].style.display = DisplayStyle.Flex; // Show the next question
        }
    }

    // Method to submit the user data to the Google Form via the GoogleFormSubmitter component 
    public void SubmitDataToSheet()
    {
        if (MeasurementDataManager.Instance != null)
        {
            //Save the data to the scriptable object in the session data
            MeasurementDataManager.Instance.SaveQuestionaireData();
        }

        else
        {
            Debug.LogError("GoogleFormSubmitter instance is not available.");
        }

    }
}