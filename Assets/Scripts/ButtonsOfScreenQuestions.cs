using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.UIElements.StyleSheets;



public class ButtonsOfScreenQuestions : MonoBehaviour
{

    private Button[] genderButtons = new Button[3];
    private Button[] q3Buttons = new Button[2];
    private Button[] q4Buttons = new Button[2];
    private Button[] q7Buttons = new Button[2];

    void Start()

    {
        // Get the UIDocument component attached to the same GameObject

        var root = GetComponent<UIDocument>().rootVisualElement;

        // Initialize gender buttons
        string[] genderButtonNames = { "MaleButton", "FemaleButton", "OtherButton" };
        for (int i = 0; i < genderButtons.Length; i++)
        {
            genderButtons[i] = root.Q<Button>(genderButtonNames[i]);
            if (genderButtons[i] == null)
                Debug.LogError($"{genderButtonNames[i]} not found.");
        }

        // Initialize Q3 buttons
        string[] q3ButtonNames = { "Q3YesOpinion", "Q3NoOpinion" };
        for (int i = 0; i < q3Buttons.Length; i++)
        {
            q3Buttons[i] = root.Q<Button>(q3ButtonNames[i]);
            if (q3Buttons[i] == null)
                Debug.LogError($"{q3ButtonNames[i]} not found.");
        }
        // Initialize Q4 buttons
        string[] q4ButtonNames = { "Q4YesOpinion", "Q4NoOpinion" };
        for (int i = 0; i < q4Buttons.Length; i++)
        {
            q4Buttons[i] = root.Q<Button>(q4ButtonNames[i]);
            if (q4Buttons[i] == null)
                Debug.LogError($"{q4ButtonNames[i]} not found.");
        }
        // Initialize Q7 buttons
        string[] q7ButtonNames = { "Q7YesOpinion", "Q7NoOpinion" };
        for (int i = 0; i < q7Buttons.Length; i++)
        {
            q7Buttons[i] = root.Q<Button>(q7ButtonNames[i]);
            if (q7Buttons[i] == null)
                Debug.LogError($"{q7ButtonNames[i]} not found.");
        }

        // Assuming gender buttons have some specific methods
        if (genderButtons[0] != null) genderButtons[0].clicked += () => OnGenderButtonClick("Male");
        if (genderButtons[1] != null) genderButtons[1].clicked += () => OnGenderButtonClick("Female");
        if (genderButtons[2] != null) genderButtons[2].clicked += () => OnGenderButtonClick("Others");

        // Assuming Q3 buttons have some specific methods
        if (q3Buttons[0] != null) q3Buttons[0].clicked += () => OnQ3ButtonClick("Yes");
        if (q3Buttons[1] != null) q3Buttons[1].clicked += () => OnQ3ButtonClick("No");

        // Assuming Q4 buttons have some specific methods
        if (q4Buttons[0] != null) q4Buttons[0].clicked += () => OnQ4ButtonClick("Yes");
        if (q4Buttons[1] != null) q4Buttons[1].clicked += () => OnQ4ButtonClick("No");

        //Assuming Q7 buttons have some specific methods
        if (q7Buttons[0] != null) q7Buttons[0].clicked += () => OnQ7ButtonClick("Yes");
        if (q7Buttons[1] != null) q7Buttons[1].clicked += () => OnQ7ButtonClick("No");




    }
    // Method to handle gender button clicks
    private void OnGenderButtonClick(string gender)
    {
        // Set the selected gender in UserDataManager
        UserDataManager.Instance.SelectedGender = gender;

        
    }

    // Method to handle Q3 button clicks
    private void OnQ3ButtonClick(string option)
    {
        // Set the selected option for Q3 in UserDataManager
        UserDataManager.Instance.Q3SelectedOption = option;

    }
    // Method to handle Q4 button clicks
    private void OnQ4ButtonClick(string option)
    {
        // Set the selected option for Q4 in UserDataManager
        UserDataManager.Instance.Q4SelectedOption = option;

    }
    // Method to handle Q7 button clicks
    private void OnQ7ButtonClick(string option)
    {
        // Set the selected option for Q7 in UserDataManager
        UserDataManager.Instance.Q7SelectedOption = option;

    }

}