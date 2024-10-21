using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;





public class UserDataManager : MonoBehaviour
{
    // Singleton instance
    public static UserDataManager Instance { get; private set; }

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

    private void Awake()
    {
        // Ensure there's only one instance of this class
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            {
                continueButtonNum = continueButtons[i];
                continueButtons[i].clicked += () => OnContinueButtonClick(index + 1);
            }
                
        }


    }
    // Method to handle the "Previous" button click for a specific question
    private void OnPreviousButtonClick(int questionIndex)
    {
        if (questionIndex == 1)
        {
            Debug.Log("PreviousButton Found");
            // Load the previous scene (e.g., the consent form)
            SceneManager.LoadScene("ConsentForm");
        }
        else if (questions[questionIndex - 2] != null && questions[questionIndex - 1] != null)
        {
            Debug.Log("I'm Working for previous");
            // Hide the current question and show the previous question
            questions[questionIndex - 2].style.display = DisplayStyle.Flex;
            questions[questionIndex - 1].style.display = DisplayStyle.None;
        }
    }
    private void OnContinueButtonClick(int questionIndex)
    {
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
            Debug.Log($"selectedOptions[{questionIndex - 1}] = {selectedOptions[questionIndex - 1]}");

            if (string.IsNullOrEmpty(Q1SelectedOption) || string.IsNullOrEmpty(SelectedDay) || string.IsNullOrEmpty(SelectedYear))
            {
                hasErrors = true;
            }
        }
        if (questionIndex >= 2 && questionIndex <= 29)
        {
            Debug.Log("Hi");

            if (questionIndex == 10)
            {
                continueButtonNum.SetEnabled(true);
            }
            else
            {
                Debug.Log($"selectedOptions[{questionIndex - 1}] = {selectedOptions[questionIndex - 1]}");
                if (string.IsNullOrEmpty(selectedOptions[questionIndex - 1]))
                {
                    Debug.Log(questionIndex - 1);
                    hasErrors = true;

                }
            }
        }
        else if (questionIndex == 30)
        {
            SubmitDataToSheet();

            SceneManager.LoadScene("Avatar_Selection");
        }

        //Update the continue button state based on whether there are errors
        continueButtonNum.SetEnabled(!hasErrors);


        if (!hasErrors)
        {
            if (questions[questionIndex - 1] != null)
                questions[questionIndex - 1].style.display = DisplayStyle.None; // Hide the current question


            if (questionIndex < questions.Length && questions[questionIndex] != null)
                questions[questionIndex].style.display = DisplayStyle.Flex; // Show the next question
        }
    }
   
    public void SubmitDataToSheet()
    {
        StartCoroutine(SubmitDataCoroutine());
    }

    private IEnumerator SubmitDataCoroutine()
    {
        WWWForm form = new WWWForm();
        // Create a dictionary to hold the Google Form entry IDs and their corresponding values
        Dictionary<string, string> formData = new Dictionary<string, string>
        {
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
        // Iterate over the dictionary and add each field to the form
        foreach (KeyValuePair<string, string> field in formData)
        {
            form.AddField(field.Key, field.Value);
        }

        // Set your Google Apps Script web app URL here
        string url = "https://docs.google.com/forms/d/e/1FAIpQLSe1CpGcYz-zV-OV8xfAl0BP4K8g2WQtLwgLaMbge6YOgMrrfw/formResponse";

        // Create the UnityWebRequest for a POST request
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("FeedBack Submitted Successfully");
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        }
    }

}
