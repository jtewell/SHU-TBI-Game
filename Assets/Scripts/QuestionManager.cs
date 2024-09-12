using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class QuestionManager : MonoBehaviour
{
    // Arrays to hold references to VisualElement and Button components for each question
    private VisualElement[] questions = new VisualElement[30];
    private Button[] previousButtons = new Button[30];
    private Button[] continueButtons = new Button[30];
  
    // This method is called when the script is enabled
    private void OnEnable()
    {
        // Get the UIDocument component attached to the same GameObject
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component is missing from the GameObject.");
            return;
        }

        // Get the root VisualElement of the UI document
        var root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("Root VisualElement is not found.");
            return;
        }

        // Initialize the question VisualElements by finding them in the UI hierarchy
        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = root.Q<VisualElement>($"Question{i + 1}");
            if (questions[i] == null)
                Debug.LogError($"Question{i + 1} element not found in the UXML.");
        }

        // Initialize the previous and continue buttons for each question
        for (int i = 0; i < previousButtons.Length; i++)
        {
            previousButtons[i] = root.Q<Button>($"Q{i + 1}PreviousButton");
            continueButtons[i] = root.Q<Button>($"Q{i + 1}Continue");

            if (previousButtons[i] == null)
                Debug.LogError($"PreviousButton for Question{i + 1} not found.");
            if (continueButtons[i] == null)
                Debug.LogError($"ContinueButton for Question{i + 1} not found.");
        }

        
        // Add click event listeners to the previous and continue buttons
        for (int i = 0; i < previousButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (previousButtons[i] != null)
                previousButtons[i].clicked += () => OnPreviousButtonClick(index + 1);
            if (continueButtons[i] != null)
                continueButtons[i].clicked += () => OnContinueButtonClick(index + 1);
        }


    }

    // Method to handle the "Previous" button click for a specific question
    private void OnPreviousButtonClick(int questionIndex)
    {
        if (questionIndex == 1)
        {
            Debug.Log("PreviousButton Found");
            // Load the previous scene (e.g., the consent form)
            SceneManager.LoadScene("ScreeningConsentForm");
        }
        else if (questions[questionIndex - 2] != null && questions[questionIndex - 1] != null)
        {
            Debug.Log("I'm Working for previous");
            // Hide the current question and show the previous question
            questions[questionIndex - 2].style.display = DisplayStyle.Flex;
            questions[questionIndex - 1].style.display = DisplayStyle.None;
        }
    }

    // Method to handle the "Continue" button click for a specific question
    private void OnContinueButtonClick(int questionIndex)
    {
        if (questions[questionIndex - 1] != null)
            questions[questionIndex - 1].style.display = DisplayStyle.None; // Hide the current question
        if (questionIndex < questions.Length && questions[questionIndex] != null)
        {
            Debug.Log("I'm Working");
            questions[questionIndex].style.display = DisplayStyle.Flex; // Show the next question
        }
        switch(questionIndex)
        {
            case 1:
                UserDataManager.Instance.LogData();
                break;
            case 2:
                UserDataManager.Instance.LogDataQ2();
                break;
            case 3:
                UserDataManager.Instance.LogDataQ3();
                break;
            case 4:
                UserDataManager.Instance.LogDataQ4();
                break;
            case 5:
                UserDataManager.Instance.LogDataQ5();
                break;
            case 6:
                UserDataManager.Instance.LogDataQ6();
                break;
            case 7:
                UserDataManager.Instance.LogDataQ7();
                break;
            case 8:
                UserDataManager.Instance.LogDataQ8();
                break;
            case 9:
                UserDataManager.Instance.LogDataQ9();
                break;
            case 10:
                break;
            case 11:
                UserDataManager.Instance.LogDataQ11();
                break;
            case 12:
                UserDataManager.Instance.LogDataQ12();
                break;
            case 13:
                UserDataManager.Instance.LogDataQ13();
                break;
            case 14:
                UserDataManager.Instance.LogDataQ14();
                break;
            case 15:
                UserDataManager.Instance.LogDataQ15();
                break;
            case 16:
                UserDataManager.Instance.LogDataQ16();
                break;
            case 17:
                UserDataManager.Instance.LogDataQ17();
                break;
            case 18:
                UserDataManager.Instance.LogDataQ18();
                break;
            case 19:
                UserDataManager.Instance.LogDataQ19();
                break;
            case 20:
                UserDataManager.Instance.LogDataQ20();
                break;
            case 21:
                UserDataManager.Instance.LogDataQ21();
                break;
            case 22:
                UserDataManager.Instance.LogDataQ22();
                break;
            case 23:
                UserDataManager.Instance.LogDataQ23();
                break;
            case 24:
                UserDataManager.Instance.LogDataQ24();
                break;
            case 25:
                UserDataManager.Instance.LogDataQ25();
                break;
            case 26:
                UserDataManager.Instance.LogDataQ26();
                break;
            case 27:
                UserDataManager.Instance.LogDataQ27();
                break;
            case 28:
                UserDataManager.Instance.LogDataQ28();
                break;
            case 29:
                UserDataManager.Instance.LogDataQ29();
                break;
            case 30:
                break;

        }

    }
}


