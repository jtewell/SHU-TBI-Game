using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.UIElements.StyleSheets;



public class RatingButtonsScreeningQuestion : MonoBehaviour
{
    // Array to hold references to the rating buttons
    private Button[] q11RatingButtons, q12RatingButtons, q13RatingButtons, q14RatingButtons, q15RatingButtons, 
        q16RatingButtons, q17RatingButtons, q18RatingButtons, q19RatingButtons, q20RatingButtons, q21RatingButtons, 
        q22RatingButtons, q23RatingButtons, q24RatingButtons, q25RatingButtons, q26RatingButtons, q27RatingButtons, 
        q28RatingButtons, q29RatingButtons;
    private Button selectedButton;




    // Start is called before the first frame update
    void Start()
    {


        // Initialize buttons for each question
        InitializeButtons(new string[] { "Q11_1", "Q11_2", "Q11_3", "Q11_4", "Q11_5" }, q11RatingButtons, 11);
        InitializeButtons(new string[] { "Q12_1", "Q12_2", "Q12_3", "Q12_4", "Q12_5" }, q12RatingButtons, 12);
        InitializeButtons(new string[] { "Q13_1", "Q13_2", "Q13_3", "Q13_4", "Q13_5" }, q13RatingButtons, 13);
        InitializeButtons(new string[] { "Q14_1", "Q14_2", "Q14_3", "Q14_4", "Q14_5" }, q14RatingButtons, 14);
        InitializeButtons(new string[] { "Q15_1", "Q15_2", "Q15_3", "Q15_4", "Q15_5" }, q15RatingButtons, 15);
        InitializeButtons(new string[] { "Q16_1", "Q16_2", "Q16_3", "Q16_4", "Q16_5" }, q16RatingButtons, 16);
        InitializeButtons(new string[] { "Q17_1", "Q17_2", "Q17_3", "Q17_4", "Q17_5" }, q17RatingButtons, 17);
        InitializeButtons(new string[] { "Q18_1", "Q18_2", "Q18_3", "Q18_4", "Q18_5" }, q18RatingButtons, 18);
        InitializeButtons(new string[] { "Q19_1", "Q19_2", "Q19_3", "Q19_4", "Q19_5" }, q19RatingButtons, 19);
        InitializeButtons(new string[] { "Q20_1", "Q20_2", "Q20_3", "Q20_4", "Q20_5" }, q20RatingButtons, 20);
        InitializeButtons(new string[] { "Q21_1", "Q21_2", "Q21_3", "Q21_4", "Q21_5" }, q21RatingButtons, 21);
        InitializeButtons(new string[] { "Q22_1", "Q22_2", "Q22_3", "Q22_4", "Q22_5" }, q22RatingButtons, 22);
        InitializeButtons(new string[] { "Q23_1", "Q23_2", "Q23_3", "Q23_4", "Q23_5" }, q23RatingButtons, 23);
        InitializeButtons(new string[] { "Q24_1", "Q24_2", "Q24_3", "Q24_4", "Q24_5" }, q24RatingButtons, 24);
        InitializeButtons(new string[] { "Q25_1", "Q25_2", "Q25_3", "Q25_4", "Q25_5" }, q25RatingButtons, 25);
        InitializeButtons(new string[] { "Q26_1", "Q26_2", "Q26_3", "Q26_4", "Q26_5" }, q26RatingButtons, 26);
        InitializeButtons(new string[] { "Q27_1", "Q27_2", "Q27_3", "Q27_4", "Q27_5" }, q27RatingButtons, 27);
        InitializeButtons(new string[] { "Q28_1", "Q28_2", "Q28_3", "Q28_4", "Q28_5" }, q28RatingButtons, 28);
        InitializeButtons(new string[] { "Q29_1", "Q29_2", "Q29_3", "Q29_4", "Q29_5" }, q29RatingButtons, 29);
    }
    void InitializeButtons(string[] buttonNames, Button[] ratingButtons, int questionNumber)
    {
        // Get the UIDocument component attached to the same GameObject
        var root = GetComponent<UIDocument>().rootVisualElement;

        ratingButtons = new Button[buttonNames.Length]; // Initialize with the correct length
        for (int i = 0; i < ratingButtons.Length; i++)
        {
            ratingButtons[i] = root.Q<Button>(buttonNames[i]);
            if (ratingButtons[i] == null)
                Debug.LogError($"{buttonNames[i]} not found.");
        }

        // Add click event listeners to the  rating buttons
        for (int i = 0; i < ratingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (ratingButtons[i] != null)
            {
                ratingButtons[i].clicked += () =>
                {
                    OnRatingButtonClick(ratingButtons[index], index + 1, questionNumber);
                };
            }
        }
    }

    void OnRatingButtonClick(Button clickedButton, int rating, int questionNumber)
    {
        // Remove the 'selected' class from the previously selected button, if any
        if (selectedButton != null)
        {
            selectedButton.RemoveFromClassList("selected");
        }

        // Set the clicked button as the selected button and apply the 'selected' class
        selectedButton = clickedButton;
        selectedButton.AddToClassList("selected");


        // Use reflection to dynamically set the selected option
        var userDataManager = ScreeningQuestionUserDataManager.Instance;
        string propertyName = $"Q{questionNumber}SelectedOption";

        // Use reflection to get the property  and set its value to the selected rating 
        var property = userDataManager.GetType().GetProperty(propertyName);
      
        if (property != null)
        {
            // Set the value of the property to the selected rating 
            property.SetValue(userDataManager, rating.ToString());
        }
        else
        {
            // Log an error if the property is not found
            Debug.LogError("Invalid question number or property not found");
        }


    }
}

