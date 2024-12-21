using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;





public class ScreeningQuestionUserDataManager : MonoBehaviour
{
    // Singleton instance
    public static ScreeningQuestionUserDataManager Instance { get; private set; }

    // User input data
    public string SelectedMonth { get; set; }
    public string SelectedDay { get; set; }
    public string SelectedYear { get; set; }
    public string SelectedGender { get; set; }
    public string Q3SelectedOption { get; set; }
    public string Q4SelectedOption { get; set; }
    public string Q5SelectedOption { get; set; }
    public string Q6SelectedOption { get; set; }
    public string Q7SelectedOption { get; set; }
    public string Q8SelectedOption { get; set; }
    public string Q9SelectedOption { get; set; }
    public string Q11SelectedOption { get; set; }
    public string Q12SelectedOption { get; set; }
    public string Q13SelectedOption { get; set; }
    public string Q14SelectedOption { get; set; }
    public string Q15SelectedOption { get; set; }
    public string Q16SelectedOption { get; set; }
    public string Q17SelectedOption { get; set; }
    public string Q18SelectedOption { get; set; }
    public string Q19SelectedOption { get; set; }
    public string Q20SelectedOption { get; set; }
    public string Q21SelectedOption { get; set; }
    public string Q22SelectedOption { get; set; }
    public string Q23SelectedOption { get; set; }
    public string Q24SelectedOption { get; set; }
    public string Q25SelectedOption { get; set; }
    public string Q26SelectedOption { get; set; }
    public string Q27SelectedOption { get; set; }
    public string Q28SelectedOption { get; set; }
    public string Q29SelectedOption { get; set; }
    public string Q30SelectedOption { get; set; }


    // Arrays to hold references to VisualElement and Button components for each question
    private VisualElement[] questions = new VisualElement[30];
    private Button[] previousButtons = new Button[30];
    private Button[] continueButtons = new Button[30];
    private VisualElement ErrorMessageContainer;
    private Button continueButtonNum;

    //keeps track of what question
    private int questionNumber = 1;

    // Unique identifier for the user (generated randomly)
    public string userId { get; private set; }


    private void Awake()
    {
        // Ensure there's only one instance of this class
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this persist across scenes

            // Generate and store the unique identifier if it doesn't exist
            if (!PlayerPrefs.HasKey("UserId"))
            {
                userId = GenerateUniqueId();
                PlayerPrefs.SetString("UserId", userId);
                PlayerPrefs.Save();
            }
            else
            {
                userId = PlayerPrefs.GetString("UserId");
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to generate a random unique identifier
    private string GenerateUniqueId()
    {
        // Generate a random number and return it as a string
        return Random.Range(100000, 999999).ToString();
    }

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
       

        if (questionNumber == 1 && !(string.IsNullOrEmpty(SelectedMonth) || string.IsNullOrEmpty(SelectedDay) || string.IsNullOrEmpty(SelectedYear)))
        {
            continueButtons[0].SetEnabled(true);
        }

        if (questionNumber == 2 && !string.IsNullOrEmpty(SelectedGender))
        {

            continueButtons[1].SetEnabled(true);
        }

        if (questionNumber > 2 && questionNumber < 30)
        {
            string[] selectedOptions = { null, null,
            Q3SelectedOption, Q4SelectedOption, Q5SelectedOption, Q6SelectedOption, Q7SelectedOption,
            Q8SelectedOption, Q9SelectedOption,"yes", Q11SelectedOption, Q12SelectedOption, Q13SelectedOption,
            Q14SelectedOption, Q15SelectedOption, Q16SelectedOption, Q17SelectedOption, Q18SelectedOption,
            Q19SelectedOption, Q20SelectedOption, Q21SelectedOption, Q22SelectedOption, Q23SelectedOption,
            Q24SelectedOption, Q25SelectedOption, Q26SelectedOption, Q27SelectedOption, Q28SelectedOption, Q29SelectedOption };

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

        string Q1SelectedOption = SelectedMonth + "/" + SelectedDay + "/" + SelectedYear;
        string Q2SelectedOption = SelectedGender;
        string[] selectedOptions = { Q1SelectedOption, Q2SelectedOption,
            Q3SelectedOption, Q4SelectedOption, Q5SelectedOption, Q6SelectedOption, Q7SelectedOption,
            Q8SelectedOption, Q9SelectedOption,"yes", Q11SelectedOption, Q12SelectedOption, Q13SelectedOption,
            Q14SelectedOption, Q15SelectedOption, Q16SelectedOption, Q17SelectedOption, Q18SelectedOption,
            Q19SelectedOption, Q20SelectedOption, Q21SelectedOption, Q22SelectedOption, Q23SelectedOption,
            Q24SelectedOption, Q25SelectedOption, Q26SelectedOption, Q27SelectedOption, Q28SelectedOption, Q29SelectedOption };

        //check if the selected option is empty
        if (questionIndex == 1)
        {
           
            //check if the selected option is empty
            if (string.IsNullOrEmpty(SelectedMonth) || string.IsNullOrEmpty(SelectedDay) || string.IsNullOrEmpty(SelectedYear))
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
        // Create a dictionary to hold the Google Form entry IDs and their corresponding values
        Dictionary<string, string> formData = new Dictionary<string, string>
        {
            { "entry.1172381183", userId },
            { "entry.1715281728", SelectedMonth },
            { "entry.629660407", SelectedDay },
            { "entry.982217643", SelectedYear },
            { "entry.1484659508", SelectedGender },
            { "entry.1012726362", Q3SelectedOption },
            { "entry.2103931096", Q4SelectedOption },
            { "entry.1340725638", Q5SelectedOption },
            { "entry.336654288", Q6SelectedOption },
            { "entry.1653943654", Q7SelectedOption },
            { "entry.248416756", Q8SelectedOption },
            { "entry.2131642439", Q9SelectedOption },
            { "entry.1654358928", Q11SelectedOption },
            { "entry.61572081", Q12SelectedOption },
            { "entry.1296288566", Q13SelectedOption },
            { "entry.1428857749", Q14SelectedOption },
            { "entry.123124654", Q15SelectedOption },
            { "entry.439547470", Q16SelectedOption },
            { "entry.2105550583", Q17SelectedOption },
            { "entry.1062803565", Q18SelectedOption },
            { "entry.258204172", Q19SelectedOption },
            { "entry.1402924908", Q20SelectedOption },
            { "entry.1183053925", Q21SelectedOption },
            { "entry.953435869", Q22SelectedOption },
            { "entry.1985824873", Q23SelectedOption },
            { "entry.1100358298", Q24SelectedOption },
            { "entry.188062488", Q25SelectedOption },
            { "entry.1061759675", Q26SelectedOption },
            { "entry.1629763265", Q27SelectedOption },
            { "entry.798811690", Q28SelectedOption },
            { "entry.1387695143", Q29SelectedOption }
        };

        // Call the GoogleFormSubmitter singleton to submit the data to the Google Form 
        if (GoogleFormSubmitter.Instance != null)
        {
            // Submit the form data to the Google Form via the GoogleFormSubmitter component 
            GoogleFormSubmitter.Instance.SubmitDataToSheet(formData);
        }
        else
        {
            Debug.LogError("GoogleFormSubmitter instance is not available.");
        }

    }
}
