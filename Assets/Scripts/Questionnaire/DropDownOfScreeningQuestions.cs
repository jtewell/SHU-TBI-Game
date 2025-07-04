using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;





public class DropDownOfScreeningQuestions : MonoBehaviour
{
    private DropdownField monthDropDown, dayDropDown, yearDropDown, Q5DropdownField, Q6DropdownField, Q8DropdownField, Q9DropdownField;

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

        // Define the choices for the dropdown
        var choicesMonth = new List<string> { "Month", "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };

        // get the current year
        int currentYear = System.DateTime.Now.Year;
        var choices1to5 = new List<string> { "1", "2", "3", "4", "5+" };

        if (monthDropDown != null)
        {
            // Set the choices
            monthDropDown.choices = choicesMonth;
            monthDropDown.value = "Month";
            monthDropDown.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.birthMonth = evt.newValue;
            });
        }
        if (dayDropDown != null)
        {
            // Define the choices for the dropdown
            var choicesDay = new List<string>();
            for (int i = 1; i <= 31; i++)
            {
                choicesDay.Add(i.ToString());
            }

            // Set the choices
            dayDropDown.choices = choicesDay;

            // Optionally, set a default value
            dayDropDown.value = "0";
            dayDropDown.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.birthDay = evt.newValue;
            });

        }
        if (yearDropDown != null)
        {
            // Define the choices for the dropdown
            var choicesDobYear = new List<string>();

            for (int i = currentYear; i >= 1900; i--)
            {
                choicesDobYear.Add(i.ToString());
            }
            yearDropDown.choices = choicesDobYear;
            // set the default value current year
            yearDropDown.value = "Year";
            yearDropDown.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.birthYear = evt.newValue;
            });
        }

        if (Q5DropdownField != null)
        {
            Q5DropdownField.choices = choices1to5;
            Q5DropdownField.value = "0";
            Q5DropdownField.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.Q5SelectedOption = evt.newValue;
            });
        }

        if (Q8DropdownField != null)
        {
            Q8DropdownField.choices = choices1to5;
            Q8DropdownField.value = "0";
            Q8DropdownField.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.Q8SelectedOption = evt.newValue;
            });
        }

        var choicesYear = new List<string>();
        // get the current year
        for (int i = currentYear; i >= 1900; i--)
        {
            choicesYear.Add(i.ToString());
        }
        if (Q6DropdownField != null)
        {
            Q6DropdownField.choices = choicesYear;
            Q6DropdownField.value = "Year";
            Q6DropdownField.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.Q6SelectedOption= evt.newValue;
            });
        }
        if (Q9DropdownField != null)
        {
            Q9DropdownField.choices = choicesYear;
            Q9DropdownField.value = "Year";

            Q9DropdownField.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.Q9SelectedOption= evt.newValue;
            });
        }
    }
}