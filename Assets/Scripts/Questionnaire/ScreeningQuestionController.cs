using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Drives the ScreeningQuestions flow: birthdate -> gender -> concussion/TBI history
/// -> severity (only if there is a history) -> last TBI date -> thank-you screen.
///
/// Every screen is described by a <see cref="Question"/> instance and the screens are
/// assembled in <see cref="BuildQuestions"/>. Options, dropdowns, validation, skipping,
/// progression and navigation are all driven off those definitions, so adding or
/// removing a screen is a change to that list - no separate button, dropdown or
/// navigation wiring required.
/// </summary>
public sealed class ScreeningQuestionController : MonoBehaviour
{
    private const string PanelPrefix = "Question";
    private const string ProgressName = "ProgressBar";
    private const string ProgressFillClass = "unity-progress-bar__progress";
    private const string SelectedClass = "selected";

    // The default dropdown selections; any of them still counts as "not answered".
    private const string MonthPlaceholder = "Month";
    private const string DayPlaceholder = "0";
    private const string YearPlaceholder = "Year";

    [Header("Scene to load when the questionnaire is finished")]
    [SerializeField] private string nextSceneName = "Avatar_Selection";

    private readonly List<Question> _questions = new();
    private readonly List<int> _visited = new(); // screen indices visited so far, in order; the last one is the current screen
    private ProgressBar _progressBar;
    private bool _started;
    private bool _finished;

    private VisualElement Root => GetComponent<UIDocument>().rootVisualElement;
    private int CurrentIndex => _visited[_visited.Count - 1];

    // ------------------------------------------------------------------ setup

    private void OnEnable()
    {
        if (_started)
            return;

        if (MeasurementDataManager.Instance == null)
        {
            Debug.LogError("ScreeningQuestionController: MeasurementDataManager.Instance is missing; the questionnaire cannot collect data.");
            return; // retried if the object is re-enabled later
        }

        var root = GetComponent<UIDocument>()?.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("ScreeningQuestionController: the UIDocument has no rootVisualElement to work with.");
            return; // retried if the object is re-enabled later
        }

        SetupProgressBar(root);

        _questions.AddRange(BuildQuestions());
        _visited.Add(0); // start on the first screen
        ShowCurrentScreen();

        _started = true;
    }

    /// <summary>
    /// Declares every screen, in order. Add, remove or reorder screens by changing
    /// this list (as long as the matching panels exist in the UXML).
    /// </summary>
    private List<Question> BuildQuestions()
    {
        var questions = new List<Question>
        {
            CreateBirthdateQuestion(),
            CreateGenderQuestion(),
            CreateConcussionHistoryQuestion(),
            CreateSeverityQuestion(),
            CreateLastTbiQuestion(),
            CreateThanksScreen(),
        };
        return questions;
    }

    private void SetupProgressBar(VisualElement root)
    {
        _progressBar = root.Q<ProgressBar>(ProgressName);
        if (_progressBar == null)
        {
            Debug.LogError($"ScreeningQuestionController: UI element '{ProgressName}' was not found in the UXML.");
            return;
        }

        _progressBar.value = 0;

        // Change the progress bar fill color to green
        var fill = _progressBar.Q(className: ProgressFillClass);
        if (fill != null)
            fill.style.backgroundColor = Color.green;
    }

    // -------------------------------------------------- question definitions

    /// <summary>Question 1: the user's birthdate (month/day/year dropdowns).</summary>
    private Question CreateBirthdateQuestion()
    {
        var question = NewQuestion(1);

        BindDateDropdowns(question,
            "monthDropDown", "dayDropDown", "yearDropDown",
            value => MeasurementDataManager.Instance.birthMonth = value,
            value => MeasurementDataManager.Instance.birthDay = value,
            value => MeasurementDataManager.Instance.birthYear = value);

        question.IsValid = () =>
            IsAnswered(MeasurementDataManager.Instance.birthMonth, MonthPlaceholder) &&
            IsAnswered(MeasurementDataManager.Instance.birthDay, DayPlaceholder) &&
            IsAnswered(MeasurementDataManager.Instance.birthYear, YearPlaceholder);

        return question;
    }

    /// <summary>Question 2: the user's gender (option buttons).</summary>
    private Question CreateGenderQuestion()
    {
        var question = NewQuestion(2);

        BindChoiceButtons(question,
            ("MaleButton", "Male"),
            ("FemaleButton", "Female"),
            // label is "Other"; the stored value keeps the legacy "Others" for data consistency
            ("OtherButton", "Others"));
        question.Save = value => MeasurementDataManager.Instance.gender = value;

        question.IsValid = () => IsAnswered(MeasurementDataManager.Instance.gender);

        return question;
    }

    /// <summary>Question 3: concussion/TBI history; a "No" answer skips the remaining history screens.</summary>
    private Question CreateConcussionHistoryQuestion()
    {
        var question = NewQuestion(3);

        BindChoiceButtons(question,
            ("Q3YesOpinion", "Yes"),
            ("Q3NoOpinion", "No"));
        question.Save = value => MeasurementDataManager.Instance.Q3SelectedOption = value;

        question.IsValid = () => IsAnswered(MeasurementDataManager.Instance.Q3SelectedOption);

        // Users without a history skip straight to the thank-you screen.
        question.ContinueTarget = current =>
            MeasurementDataManager.Instance.Q3SelectedOption == "No" ? _questions.Count - 1 : current + 1;

        return question;
    }

    /// <summary>Question 4: severity of the concussion/TBI (option buttons).</summary>
    private Question CreateSeverityQuestion()
    {
        var question = NewQuestion(4);

        BindChoiceButtons(question,
            ("Q4MildOpinion", "Mild"),
            ("Q4ModerateOpinion", "Moderate"),
            ("Q4SevereOpinion", "Severe"),
            ("Q4IDKOpinion", "Not sure"));
        question.Save = value => MeasurementDataManager.Instance.Q4SelectedOption = value;

        question.IsValid = () => IsAnswered(MeasurementDataManager.Instance.Q4SelectedOption);

        return question;
    }

    /// <summary>Question 5: how long ago the last TBI was (month/day/year dropdowns).</summary>
    private Question CreateLastTbiQuestion()
    {
        var question = NewQuestion(5);

        BindDateDropdowns(question,
            "TBIMonth", "TBIDay", "TBIYear",
            value => MeasurementDataManager.Instance.TBIMonth = value,
            value => MeasurementDataManager.Instance.TBIDay = value,
            value => MeasurementDataManager.Instance.TBIYear = value);

        question.IsValid = () =>
            IsAnswered(MeasurementDataManager.Instance.TBIMonth, MonthPlaceholder) &&
            IsAnswered(MeasurementDataManager.Instance.TBIDay, DayPlaceholder) &&
            IsAnswered(MeasurementDataManager.Instance.TBIYear, YearPlaceholder);

        return question;
    }

    /// <summary>Question 6: thank-you screen; Continue saves the data and moves to the next scene.</summary>
    private Question CreateThanksScreen()
    {
        var question = NewQuestion(6);
        question.OnFinish = Finish;
        return question;
    }

    // ---------------------------------------------------------------- binding

    private Question NewQuestion(int number)
    {
        var root = Root;

        var panel = root.Q<VisualElement>($"{PanelPrefix}{number}");
        if (panel == null)
            Debug.LogError($"ScreeningQuestionController: panel '{PanelPrefix}{number}' was not found in the UXML.");

        var question = new Question
        {
            Number = number,
            Panel = panel,
        };

        question.Continue = FindButton(root, question, $"Q{number}Continue");
        if (question.Continue != null)
            question.Continue.clicked += OnContinue;

        // The first screen has no previous button.
        if (number > 1)
        {
            question.Previous = FindButton(root, question, $"Q{number}PreviousButton");
            if (question.Previous != null)
                question.Previous.clicked += OnPrevious;
        }

        return question;
    }

    /// <summary>Wires buttons that all record one chosen value (gender, yes/no, severity, ...).</summary>
    private void BindChoiceButtons(Question question, params (string Button, string Value)[] choices)
    {
        var root = Root;

        foreach (var (buttonName, value) in choices)
        {
            var option = new Option
            {
                Value = value,
                Button = FindButton(root, question, buttonName),
            };
            if (option.Button == null)
                continue;

            question.Options.Add(option);
            option.Button.clicked += () => SelectOption(question, option);
        }
    }

    /// <summary>Wires the month/day/year dropdowns of a date question (used for both the birthdate and the last TBI date).</summary>
    private void BindDateDropdowns(Question question,
        string monthName, string dayName, string yearName,
        Action<string> saveMonth, Action<string> saveDay, Action<string> saveYear)
    {
        var root = Root;

        BindDropdown(root, question, monthName, MonthChoices(), MonthPlaceholder, saveMonth);
        BindDropdown(root, question, dayName, DayChoices(), DayPlaceholder, saveDay);
        BindDropdown(root, question, yearName, YearChoices(), YearPlaceholder, saveYear);
    }

    private void BindDropdown(VisualElement root, Question question, string name,
        List<string> choices, string placeholder, Action<string> save)
    {
        var dropdown = root.Q<DropdownField>(name);
        if (dropdown == null)
        {
            Debug.LogError($"ScreeningQuestionController: dropdown '{name}' for question {question.Number} was not found in the UXML.");
            return;
        }

        dropdown.choices = choices;
        dropdown.value = placeholder; // defaults to the placeholder, which never counts as an answer
        dropdown.RegisterValueChangedCallback(evt =>
        {
            save(evt.newValue);
            UpdateContinueButtons();
        });
    }

    private static List<string> MonthChoices() => new()
    {
        "Month", "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"
    };

    private static List<string> DayChoices() =>
        Enumerable.Range(1, 31).Select(day => day.ToString()).ToList();

    private static List<string> YearChoices()
    {
        var choices = new List<string>();
        for (int year = DateTime.Now.Year; year >= 1900; year--)
            choices.Add(year.ToString());
        return choices;
    }

    // ------------------------------------------------------------- navigation

    private void OnContinue()
    {
        var question = _questions[CurrentIndex];

        if (question.OnFinish != null)
        {
            question.OnFinish();
            return;
        }

        // Defensive only: the continue button is disabled until the question has an answer.
        if (question.IsValid != null && !question.IsValid())
            return;

        var target = question.ContinueTarget != null ? question.ContinueTarget(CurrentIndex) : CurrentIndex + 1;
        NavigateTo(Mathf.Clamp(target, 0, _questions.Count - 1));
    }

    private void OnPrevious()
    {
        if (_visited.Count <= 1)
            return;

        // Drop the current screen and return to the one before it; the path makes this
        // work even when screens were skipped (e.g. straight back from the thank-you screen to Q3).
        _visited.RemoveAt(_visited.Count - 1);
        ShowCurrentScreen();
    }

    private void NavigateTo(int index)
    {
        if (index == CurrentIndex)
            return;

        _visited.Add(index);
        ShowCurrentScreen();
    }

    /// <summary>Syncs the visible panel, the progress bar and the continue buttons with the current screen.</summary>
    private void ShowCurrentScreen()
    {
        for (int i = 0; i < _questions.Count; i++)
        {
            var panel = _questions[i].Panel;
            if (panel == null)
                continue;

            panel.style.display = i == CurrentIndex ? DisplayStyle.Flex : DisplayStyle.None;
        }

        UpdateProgressBar();
        UpdateContinueButtons();
    }

    private void UpdateProgressBar()
    {
        if (_progressBar == null)
            return;

        // Position-based progression: first screen 0%, last screen 100%.
        float steps = _questions.Count - 1;
        _progressBar.value = steps > 0 ? CurrentIndex * Mathf.Abs(100f / steps) : 0f;
    }

    private void UpdateContinueButtons()
    {
        for (int i = 0; i < _questions.Count; i++)
        {
            var question = _questions[i];
            if (question.Continue == null)
                continue;

            // Only the current screen can be continued, and only once it does not need an
            // answer or it has one.
            question.Continue.SetEnabled(i == CurrentIndex && (question.IsValid == null || question.IsValid()));
        }
    }

    private void Finish()
    {
        if (_finished)
            return;
        _finished = true;

        if (MeasurementDataManager.Instance == null)
        {
            Debug.LogError("ScreeningQuestionController: MeasurementDataManager.Instance is missing; the questionnaire was not saved.");
            return;
        }

        // Save the questionnaire answers into the session data scriptable object, then move on.
        MeasurementDataManager.Instance.SaveQuestionaireData();
        SceneManager.LoadScene(nextSceneName);
    }

    // ---------------------------------------------------------------- helpers

    private Button FindButton(VisualElement root, Question question, string name)
    {
        var button = root.Q<Button>(name);
        if (button == null)
            Debug.LogError($"ScreeningQuestionController: button '{name}' of question {question.Number} was not found in the UXML.");
        return button;
    }

    private void SelectOption(Question question, Option option)
    {
        if (question.SelectedOption != null)
            question.SelectedOption.RemoveFromClassList(SelectedClass);

        question.SelectedOption = option.Button;
        option.Button.AddToClassList(SelectedClass);

        question.Save?.Invoke(option.Value);
        UpdateContinueButtons();
    }

    private static bool IsAnswered(string value, string placeholder = null) =>
        !string.IsNullOrEmpty(value) && value != placeholder;

    // -------------------------------------------------------- nested types

    private sealed class Question
    {
        public int Number; // 1-based; matches the UXML naming (Question{n}, Q{n}Continue, ...)
        public VisualElement Panel;
        public Button Previous;
        public Button Continue;
        public Button SelectedOption; // option button currently carrying the "selected" class
        public Action<string> Save; // records the answer when an option is chosen
        public Func<bool> IsValid; // null => continuing doesn't require an answer (thank-you screen)
        public Func<int, int> ContinueTarget; // optional override of where Continue goes (0-based index); null => the next screen
        public Action OnFinish; // when set, Continue runs this instead of navigating (thank-you screen)
        public readonly List<Option> Options = new();
    }

    private sealed class Option
    {
        public string Value;
        public Button Button;
    }
}
