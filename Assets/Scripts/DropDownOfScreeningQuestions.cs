using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.UIElements.StyleSheets;
using UnityEditor;
using UnityEngine.UI;




public class DropDownOfScreeningQuestions : MonoBehaviour
{
    private DropdownField monthDropDown;
    private DropdownField dayDropDown;
    private DropdownField yearDropDown;
    private DropdownField Q5DropdownField;
    private DropdownField Q6DropdownField;
    private DropdownField Q8DropdownField;
    private DropdownField Q9DropdownField;

    // Start is called before the first frame update
    void Start()
    {
         
        var root = GetComponent<UIDocument>().rootVisualElement;
        monthDropDown = root.Q<DropdownField>("monthDropDown");
        dayDropDown = root.Q<DropdownField>("dayDropDown");
        yearDropDown = root.Q<DropdownField>("yearDropDown");
        Q5DropdownField = root.Q<DropdownField>("Q5DropdownField");
        Q6DropdownField = root.Q<DropdownField>("Q6DropdownField");
        Q8DropdownField = root.Q<DropdownField>("Q8DropdownField");
        Q9DropdownField = root.Q<DropdownField>("Q9DropdownField");




        if (monthDropDown != null)
        {
            // Define the choices for the dropdown
            var choices = new List<string> { "Month", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            // Set the choices
            monthDropDown.choices = choices;
            monthDropDown.value = "Month";

            // Register a callback for when the value changes 
            monthDropDown.RegisterValueChangedCallback(evt => {
                UserDataManager.Instance.SelectedMonth = evt.newValue;
            });
        }
        if (dayDropDown != null)
        {
            // Define the choices for the dropdown
            var choices = new List<string>();
            for (int i = 1; i <= 31; i++)
            {
                choices.Add(i.ToString());
            }

            // Set the choices
            dayDropDown.choices = choices;

            // Optionally, set a default value
            dayDropDown.value = "0";

            // Register a callback for when the value changes
            dayDropDown.RegisterValueChangedCallback(evt =>
            {
                UserDataManager.Instance.SelectedDay = evt.newValue;
            });

        }
        if (yearDropDown != null)
        {
            // Define the choices for the dropdown
            var choices = new List<string>();
            // get the current year
            int currentYear = System.DateTime.Now.Year;
            for (int i = currentYear; i >= 1900; i--)
            {
                choices.Add(i.ToString());
            }
            yearDropDown.choices = choices;
            // set the default value current year
            yearDropDown.value = currentYear.ToString();

            // Register a callback for when the value changes
            yearDropDown.RegisterValueChangedCallback(evt =>
            {
                UserDataManager.Instance.SelectedYear = evt.newValue;
            });

        }
        





        if (Q5DropdownField != null)
        {
            // Define the choices for the dropdown
            var choices = new List<string> { "1", "2","3","4","5+"};

            // Set the choices
            Q5DropdownField.choices = choices;

            // Optionally, set a default value
            Q5DropdownField.value = "0";

            // Register a callback for when the value changes 
            Q5DropdownField.RegisterValueChangedCallback(evt =>
            {
                UserDataManager.Instance.Q5SelectedOption = evt.newValue;
            });
        }
        if (Q6DropdownField !=null)
        {
            // Define the choices for the dropdown
            var choices = new List<string>();
            // get the current year
            int currentYear = System.DateTime.Now.Year;
            for (int i = currentYear; i >= 1900; i--)
            {
                choices.Add(i.ToString());
            }
            Q6DropdownField.choices = choices;
            // set the default value current year
            Q6DropdownField.value = currentYear.ToString();

            // Register a callback for when the value changes
            Q6DropdownField.RegisterValueChangedCallback(evt =>
            {
                UserDataManager.Instance.Q6SelectedOption = evt.newValue;
            });
        }
        if (Q8DropdownField != null)
        {
            // Define the choices for the dropdown
            var choices = new List<string> { "1", "2", "3", "4", "5+" };

            // Set the choices
            Q8DropdownField.choices = choices;

            // Optionally, set a default value
            Q8DropdownField.value = "0";

            // Register a callback for when the value changes 
            Q8DropdownField.RegisterValueChangedCallback(evt =>
            {
                UserDataManager.Instance.Q8SelectedOption = evt.newValue;
            });
        }
        if (Q9DropdownField != null)
        {
            // Define the choices for the dropdown
            var choices = new List<string>();
            // get the current year
            int currentYear = System.DateTime.Now.Year;
            for (int i = currentYear; i >= 1900; i--)
            {
                choices.Add(i.ToString());
            }
            Q9DropdownField.choices = choices;
            // set the default value current year
            Q9DropdownField.value = currentYear.ToString();

            // Register a callback for when the value changes
            Q9DropdownField.RegisterValueChangedCallback(evt =>
            {
                UserDataManager.Instance.Q9SelectedOption = evt.newValue;
            });
        }

    }

    // Update is called once per frame
    
}
