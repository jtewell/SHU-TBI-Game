using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class DropDownOfScreeningQuestions : MonoBehaviour
{
    private DropdownField monthDropDown, dayDropDown, yearDropDown, TBIMonth, TBIDay, TBIYear;

    // Start is called before the first frame update
    void Start()
    {

        var root = GetComponent<UIDocument>().rootVisualElement;
        monthDropDown = root.Q<DropdownField>("monthDropDown");
        dayDropDown = root.Q<DropdownField>("dayDropDown");
        yearDropDown = root.Q<DropdownField>("yearDropDown");
        TBIMonth = root.Q<DropdownField>("TBIMonth");
        TBIDay = root.Q<DropdownField>("TBIDay");
        TBIYear = root.Q<DropdownField>("TBIYear");


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

        if (TBIMonth != null)
        {
            TBIMonth.choices = choicesMonth;
            TBIMonth.value = "Month";
            TBIMonth.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.TBIMonth = evt.newValue;
            });
        }

        if (TBIDay != null)
        {
            // Define the choices for the dropdown
            var choicesDay = new List<string>();
            for (int i = 1; i <= 31; i++)
            {
                choicesDay.Add(i.ToString());
            }

            // Set the choices
            TBIDay.choices = choicesDay;

            // Optionally, set a default value
            TBIDay.value = "0";
            TBIDay.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.TBIDay = evt.newValue;
            });
        }

        if (TBIYear != null)
        {
            // Define the choices for the dropdown
            var choicesTBIYear = new List<string>();
            for (int i = currentYear; i >= 1900; i--)
            {
                choicesTBIYear.Add(i.ToString());
            }
            TBIYear.choices = choicesTBIYear;
            // set the default value current year
            TBIYear.value = "Year";
            TBIYear.RegisterValueChangedCallback(evt =>
            {
                MeasurementDataManager.Instance.TBIYear = evt.newValue;
            });
        }
    }
}