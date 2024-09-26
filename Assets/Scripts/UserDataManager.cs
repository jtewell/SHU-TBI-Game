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

    private string scriptUrl = "https://script.google.com/macros/s/AKfycbyh4seaBy4Ev_XH0fTAEAJYo48cFL_2tv48LZBbNA6h5kpT8365CEZWORD9enET7X2J/exec";

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
                continueButtonNum = continueButtons[i];
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

    // Method to handle the "Continue" button click for a specific question
    private void OnContinueButtonClick(int questionIndex)
    {
        bool hasErrors = false; // Flag to track if there are validation errors

        switch (questionIndex)
        {
            case 1:
                if (string.IsNullOrEmpty(SelectedMonth) || string.IsNullOrEmpty(SelectedDay) || string.IsNullOrEmpty(SelectedYear))
                {
                    
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                    
                }
                else
                {
                    
                    UserDataManager.Instance.LogData();
                }
                break;

            case 2:
                if (string.IsNullOrEmpty(SelectedGender))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ2();
                }
                break;
            case 3:
                if (string.IsNullOrEmpty(Q3SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ3();
                }
                break;
            case 4:
                if (string.IsNullOrEmpty(Q4SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ4();
                }
                break;
            case 5:
                if (string.IsNullOrEmpty(Q5SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    SubmitDataToSheet();
                    UserDataManager.Instance.LogDataQ5();
                }
                break;
            case 6:
                if (string.IsNullOrEmpty(Q6SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ6();
                }
                break;
            case 7:
                if (string.IsNullOrEmpty(Q7SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ7();
                }
                break;
            case 8:
                if (string.IsNullOrEmpty(Q8SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ8();
                }
                break;
            case 9:
                if (string.IsNullOrEmpty(Q9SelectedOption))
                {
                   hasErrors = true;
                   continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {
                    
                    UserDataManager.Instance.LogDataQ9();
                }
                break;
            case 10:
                break;
            case 11:
                if (string.IsNullOrEmpty(Q11SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ11();
                }
                break;
            case 12:
                if (string.IsNullOrEmpty(Q12SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ12();
                }

                break;
            case 13:
                if (string.IsNullOrEmpty(Q13SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ13();
                }

                break;
            case 14:
                if (string.IsNullOrEmpty(Q14SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ14();
                }
                break;
            case 15:
                if (string.IsNullOrEmpty(Q15SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ15();
                }
                break;
            case 16:
                if (string.IsNullOrEmpty(Q16SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ16();
                }
                break;
            case 17:
                if (string.IsNullOrEmpty(Q17SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ17();
                }
                break;
            case 18:
                if (string.IsNullOrEmpty(Q18SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ18();
                }
                break;
            case 19:
                if (string.IsNullOrEmpty(Q19SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ19();
                }
                break;
            case 20:
                if (string.IsNullOrEmpty(Q20SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ20();
                }
                break;
            case 21:
                if (string.IsNullOrEmpty(Q21SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ21();
                }
                break;
            case 22:
                if (string.IsNullOrEmpty(Q22SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ22();
                }
                break;
            case 23:
                if (string.IsNullOrEmpty(Q23SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ23();
                }
                break;
            case 24:
                if (string.IsNullOrEmpty(Q24SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ24();
                }
                break;
            case 25:
                if (string.IsNullOrEmpty(Q25SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ25();
                }
                break;
            case 26:
                if (string.IsNullOrEmpty(Q26SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ26();
                }
                break;
            case 27:
                if (string.IsNullOrEmpty(Q27SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ27();
                }
                break;
            case 28:
                if (string.IsNullOrEmpty(Q28SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ28();
                }
                break;
            case 29:
                if (string.IsNullOrEmpty(Q29SelectedOption))
                {
                    hasErrors = true;
                    continueButtonNum.SetEnabled(!hasErrors);
                }
                else
                {

                    UserDataManager.Instance.LogDataQ29();
                    SceneManager.LoadScene("Avatar_Selection");
                }
                break;
            case 30:
                //if (string.IsNullOrEmpty(Q30SelectedOption))
                //{
                //    hasErrors = true;
                //    continueButtonNum.SetEnabled(!hasErrors);
                //}
                //else
                //{
                //}
                break;

        }
        if (!hasErrors)
        {
            if (questions[questionIndex - 1] != null)

            
            questions[questionIndex - 1].style.display = DisplayStyle.None; // Hide the current question

            if (questionIndex < questions.Length && questions[questionIndex] != null)
            {
                Debug.Log("I'm Working");
                questions[questionIndex].style.display = DisplayStyle.Flex; // Show the next question
            }
        }
        
    }
    public void SubmitDataToSheet()
    {
        StartCoroutine(SubmitDataCoroutine());
    }

    private IEnumerator SubmitDataCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("data", JsonUtility.ToJson(new
        {
            month = SelectedMonth,
            day = SelectedDay,
            year = SelectedYear,
            gender = SelectedGender,
            q3 = Q3SelectedOption,
            q4 = Q4SelectedOption,
            q5 = Q5SelectedOption

        }));

        using (UnityWebRequest www = UnityWebRequest.Post(scriptUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data successfully submitted!");
            }
            else
            {
                Debug.LogError($"Error: {www.error}");
            }
        }
    }

    // Method to log all data to the console

    public void LogData()
    {
        Debug.Log("Hi,I'm from user data manager");
        Debug.Log($"DOB: {SelectedMonth} {SelectedDay}, {SelectedYear}");
    }
    public void LogDataQ2()
    {
        Debug.Log("Hi,I'm from user data manager Q2");
        Debug.Log($"Gender: {SelectedGender}");
    }
    public void LogDataQ3()
    {
        Debug.Log("Hi,I'm from user data manager Q3");
        Debug.Log($"Q3: {Q3SelectedOption}");
    }
    public void LogDataQ4()
    {
        Debug.Log("Hi,I'm from user data manager Q4");
        Debug.Log($"Q4: {Q4SelectedOption}");
    }
    public void LogDataQ5()
    {
        Debug.Log("Hi,I'm from user data manager Q5");
        Debug.Log($"Q5: {Q5SelectedOption}");
    }
    public void LogDataQ6()
    {
        Debug.Log("Hi,I'm from user data manager Q6");
        Debug.Log($"Q6: {Q6SelectedOption}");
    }
    public void LogDataQ7()
    {
        Debug.Log("Hi,I'm from user data manager Q7");
        Debug.Log($"Q7: {Q7SelectedOption}");
    }
    public void LogDataQ8()
    {
        Debug.Log("Hi,I'm from user data manager Q8");
        Debug.Log($"Q8: {Q8SelectedOption}");
    }
    public void LogDataQ9()
    {
        Debug.Log("Hi,I'm from user data manager Q9");
        Debug.Log($"Q9: {Q9SelectedOption}");
    }
    public void LogDataQ11()
    {
        Debug.Log("Hi,I'm from user data manager Q11");
        Debug.Log($"Q11: {Q11SelectedOption}");
    }
    public void LogDataQ12()
    {
        Debug.Log("Hi,I'm from user data manager Q12");
        Debug.Log($"Q12: {Q12SelectedOption}");
    }
    public void LogDataQ13()
    {
        Debug.Log("Hi,I'm from user data manager Q13");
        Debug.Log($"Q13: {Q13SelectedOption}");
    }
    public void LogDataQ14()
    {
        Debug.Log("Hi,I'm from user data manager Q14");
        Debug.Log($"Q14: {Q14SelectedOption}");
    }
    public void LogDataQ15()
    {
        Debug.Log("Hi,I'm from user data manager Q15");
        Debug.Log($"Q15: {Q15SelectedOption}");
    }
    public void LogDataQ16()
    {
        Debug.Log("Hi,I'm from user data manager Q16");
        Debug.Log($"Q16: {Q16SelectedOption}");
    }
    public void LogDataQ17()
    {
        Debug.Log("Hi,I'm from user data manager Q17");
        Debug.Log($"Q17: {Q17SelectedOption}");
    }
    public void LogDataQ18()
    {
        Debug.Log("Hi,I'm from user data manager Q18");
        Debug.Log($"Q18: {Q18SelectedOption}");
    }
    public void LogDataQ19()
    {
        Debug.Log("Hi,I'm from user data manager Q19");
        Debug.Log($"Q19: {Q19SelectedOption}");
    }
    public void LogDataQ20()
    {
        Debug.Log("Hi,I'm from user data manager Q20");
        Debug.Log($"Q20: {Q20SelectedOption}");
    }
    public void LogDataQ21()
    {
        Debug.Log("Hi,I'm from user data manager Q21");
        Debug.Log($"Q21: {Q21SelectedOption}");
    }

    public void LogDataQ22()
    {
        Debug.Log("Hi,I'm from user data manager Q22");
        Debug.Log($"Q22: {Q22SelectedOption}");
    }
    public void LogDataQ23()
    {
        Debug.Log("Hi,I'm from user data manager Q23");
        Debug.Log($"Q23: {Q23SelectedOption}");
    }
    public void LogDataQ24()
    {
        Debug.Log("Hi,I'm from user data manager Q24");
        Debug.Log($"Q24: {Q24SelectedOption}");
    }
    public void LogDataQ25()
    {
        Debug.Log("Hi,I'm from user data manager Q25");
        Debug.Log($"Q25: {Q25SelectedOption}");
    }
    public void LogDataQ26()
    {
        Debug.Log("Hi,I'm from user data manager Q26");
        Debug.Log($"Q26: {Q26SelectedOption}");
    }
    public void LogDataQ27()
    {
        Debug.Log("Hi,I'm from user data manager Q27");
        Debug.Log($"Q27: {Q27SelectedOption}");
    }
    public void LogDataQ28()
    {
        Debug.Log("Hi,I'm from user data manager Q28");
        Debug.Log($"Q28: {Q28SelectedOption}");
    }
    public void LogDataQ29()
    {
        Debug.Log("Hi,I'm from user data manager Q29");
        Debug.Log($"Q29: {Q29SelectedOption}");
    }

}
