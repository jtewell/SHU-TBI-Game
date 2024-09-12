using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.UIElements.StyleSheets;



public class RatingButtonsScreeningQuestion : MonoBehaviour
{
    // Array to hold references to the rating buttons
    private Button[] q11RatingButtons = new Button[5];
    private Button[] q12RatingButtons = new Button[5];
    private Button[] q13RatingButtons = new Button[5];
    private Button[] q14RatingButtons = new Button[5];
    private Button[] q15RatingButtons = new Button[5];
    private Button[] q16RatingButtons = new Button[5];
    private Button[] q17RatingButtons = new Button[5];
    private Button[] q18RatingButtons = new Button[5];
    private Button[] q19RatingButtons = new Button[5];
    private Button[] q20RatingButtons = new Button[5];
    private Button[] q21RatingButtons = new Button[5];
    private Button[] q22RatingButtons = new Button[5];
    private Button[] q23RatingButtons = new Button[5];
    private Button[] q24RatingButtons = new Button[5];
    private Button[] q25RatingButtons = new Button[5];
    private Button[] q26RatingButtons = new Button[5];
    private Button[] q27RatingButtons = new Button[5];
    private Button[] q28RatingButtons = new Button[5];
    private Button[] q29RatingButtons = new Button[5];
    private Button[] q30RatingButtons = new Button[5];




    // Start is called before the first frame update
    void Start()
    {
        // Get the UIDocument component attached to the same GameObject
        var root = GetComponent<UIDocument>().rootVisualElement;
        
    // Initialize Q11 rating buttons

        string[] q11RatingButtonNames = { "Q11_1", "Q11_2", "Q11_3", "Q11_4", "Q11_5" };
        for (int i = 0; i < q11RatingButtons.Length; i++)
        {
            q11RatingButtons[i] = root.Q<Button>(q11RatingButtonNames[i]);
            if (q11RatingButtons[i] == null)
            Debug.LogError($"{q11RatingButtonNames[i]} not found.");
        }
        string[] q12RatingButtonNames = { "Q12_1", "Q12_2", "Q12_3", "Q12_4", "Q12_5" };
        for (int i = 0; i < q12RatingButtons.Length; i++)
        {
            q12RatingButtons[i] = root.Q<Button>(q12RatingButtonNames[i]);
            if (q12RatingButtons[i] == null)
            Debug.LogError($"{q12RatingButtonNames[i]} not found.");
        }
        string[] q13RatingButtonNames = { "Q13_1", "Q13_2", "Q13_3", "Q13_4", "Q13_5" };
        for (int i = 0; i < q13RatingButtons.Length; i++)
        {
            q13RatingButtons[i] = root.Q<Button>(q13RatingButtonNames[i]);
            if (q13RatingButtons[i] == null)
            Debug.LogError($"{q13RatingButtonNames[i]} not found.");
        }
        string[] q14RatingButtonNames = { "Q14_1", "Q14_2", "Q14_3", "Q14_4", "Q14_5" };
        for (int i = 0; i < q14RatingButtons.Length; i++)
        {
            q14RatingButtons[i] = root.Q<Button>(q14RatingButtonNames[i]);
            if (q14RatingButtons[i] == null)
            Debug.LogError($"{q14RatingButtonNames[i]} not found.");
        }
        string[] q15RatingButtonNames = { "Q15_1", "Q15_2", "Q15_3", "Q15_4", "Q15_5" };
        for (int i = 0; i < q15RatingButtons.Length; i++)
        {
            q15RatingButtons[i] = root.Q<Button>(q15RatingButtonNames[i]);
            if (q15RatingButtons[i] == null)
            Debug.LogError($"{q15RatingButtonNames[i]} not found.");
        }
        string[] q16RattingButtonNames = { "Q16_1", "Q16_2", "Q16_3", "Q16_4", "Q16_5" };
         for (int i = 0; i < q16RatingButtons.Length; i++)
         {
            q16RatingButtons[i] = root.Q<Button>(q16RattingButtonNames[i]);
            if (q16RatingButtons[i] == null)
            Debug.LogError($"{q16RattingButtonNames[i]} not found.");

         }
        string[] q17RattingButtonNames = { "Q17_1", "Q17_2", "Q17_3", "Q17_4", "Q17_5" };
        for (int i = 0; i < q17RatingButtons.Length; i++)
        {
            q17RatingButtons[i] = root.Q<Button>(q17RattingButtonNames[i]);
            if (q17RatingButtons[i] == null)
                Debug.LogError($"{q17RattingButtonNames[i]} not found.");

        }
        string[] q18RattingButtonNames = { "Q18_1", "Q18_2", "Q18_3", "Q18_4", "Q18_5" };
        for (int i = 0; i < q18RatingButtons.Length; i++)
        {
            q18RatingButtons[i] = root.Q<Button>(q18RattingButtonNames[i]);
            if (q18RatingButtons[i] == null)
                Debug.LogError($"{q18RattingButtonNames[i]} not found.");

        }
        string[] q19RattingButtonNames = { "Q19_1", "Q19_2", "Q19_3", "Q19_4", "Q19_5" };
        for (int i = 0; i < q19RatingButtons.Length; i++)
        {
            q19RatingButtons[i] = root.Q<Button>(q19RattingButtonNames[i]);
            if (q19RatingButtons[i] == null)
                Debug.LogError($"{q19RattingButtonNames[i]} not found.");

        }
        string[] q20RattingButtonNames = { "Q20_1", "Q20_2", "Q20_3", "Q20_4", "Q20_5" };
        for (int i = 0; i < q20RatingButtons.Length; i++)
        {
            q20RatingButtons[i] = root.Q<Button>(q20RattingButtonNames[i]);
            if (q20RatingButtons[i] == null)
                Debug.LogError($"{q20RattingButtonNames[i]} not found.");

        }
        string[] q21RattingButtonNames = { "Q21_1", "Q21_2", "Q21_3", "Q21_4", "Q21_5" };
        for (int i = 0; i < q21RatingButtons.Length; i++)
        {
            q21RatingButtons[i] = root.Q<Button>(q21RattingButtonNames[i]);
            if (q21RatingButtons[i] == null)
                Debug.LogError($"{q21RattingButtonNames[i]} not found.");

        }
        string[] q22RattingButtonNames = { "Q22_1", "Q22_2", "Q22_3", "Q22_4", "Q22_5" };
        for (int i = 0; i < q22RatingButtons.Length; i++)
        {
            q22RatingButtons[i] = root.Q<Button>(q22RattingButtonNames[i]);
            if (q22RatingButtons[i] == null)
                Debug.LogError($"{q22RattingButtonNames[i]} not found.");

        }
        string[] q23RattingButtonNames = { "Q23_1", "Q23_2", "Q23_3", "Q23_4", "Q23_5" };
        for (int i = 0; i < q23RatingButtons.Length; i++)
        {
            q23RatingButtons[i] = root.Q<Button>(q23RattingButtonNames[i]);
            if (q23RatingButtons[i] == null)
                Debug.LogError($"{q23RattingButtonNames[i]} not found.");

        }
        string[] q24RattingButtonNames = { "Q24_1", "Q24_2", "Q24_3", "Q24_4", "Q24_5" };
        for (int i = 0; i < q24RatingButtons.Length; i++)
        {
            q24RatingButtons[i] = root.Q<Button>(q24RattingButtonNames[i]);
            if (q24RatingButtons[i] == null)
                Debug.LogError($"{q24RattingButtonNames[i]} not found.");

        }
        string[] q25RattingButtonNames = { "Q25_1", "Q25_2", "Q25_3", "Q25_4", "Q25_5" };
        for (int i = 0; i < q25RatingButtons.Length; i++)
        {
            q25RatingButtons[i] = root.Q<Button>(q25RattingButtonNames[i]);
            if (q25RatingButtons[i] == null)
                Debug.LogError($"{q25RattingButtonNames[i]} not found.");

        }
        string[] q26RattingButtonNames = { "Q26_1", "Q26_2", "Q26_3", "Q26_4", "Q26_5" };
        for (int i = 0; i < q26RatingButtons.Length; i++)
        {
            q26RatingButtons[i] = root.Q<Button>(q26RattingButtonNames[i]);
            if (q26RatingButtons[i] == null)
                Debug.LogError($"{q26RattingButtonNames[i]} not found.");

        }
        string[] q27RattingButtonNames = { "Q27_1", "Q27_2", "Q27_3", "Q27_4", "Q27_5" };
        for (int i = 0; i < q27RatingButtons.Length; i++)
        {
            q27RatingButtons[i] = root.Q<Button>(q27RattingButtonNames[i]);
            if (q27RatingButtons[i] == null)
                Debug.LogError($"{q27RattingButtonNames[i]} not found.");

        }
        string[] q28RattingButtonNames = { "Q28_1", "Q28_2", "Q28_3", "Q28_4", "Q28_5" };

        for (int i = 0; i < q28RatingButtons.Length; i++)
        {
            q28RatingButtons[i] = root.Q<Button>(q28RattingButtonNames[i]);
            if (q28RatingButtons[i] == null)
                Debug.LogError($"{q28RattingButtonNames[i]} not found.");

        }
        string[] q29RattingButtonNames = { "Q29_1", "Q29_2", "Q29_3", "Q29_4", "Q29_5" };
        for (int i = 0; i < q29RatingButtons.Length; i++)
        {
            q29RatingButtons[i] = root.Q<Button>(q29RattingButtonNames[i]);
            if (q29RatingButtons[i] == null)
                Debug.LogError($"{q29RattingButtonNames[i]} not found.");

        }


        // Add click event listeners to the  rating buttons


        for (int i = 0; i < q11RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q11RatingButtons[i] != null)
                q11RatingButtons[i].clicked += () => OnQ11RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q12RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q12RatingButtons[i] != null)
                q12RatingButtons[i].clicked += () => OnQ12RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q13RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q13RatingButtons[i] != null)
                q13RatingButtons[i].clicked += () => OnQ13RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q14RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q14RatingButtons[i] != null)
                q14RatingButtons[i].clicked += () => OnQ14RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q15RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q15RatingButtons[i] != null)
                q15RatingButtons[i].clicked += () => OnQ15RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q16RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q16RatingButtons[i] != null)
                q16RatingButtons[i].clicked += () => OnQ16RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q17RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q17RatingButtons[i] != null)
                q17RatingButtons[i].clicked += () => OnQ17RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q18RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q18RatingButtons[i] != null)
                q18RatingButtons[i].clicked += () => OnQ18RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q19RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q19RatingButtons[i] != null)
                q19RatingButtons[i].clicked += () => OnQ19RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q20RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q20RatingButtons[i] != null)
                q20RatingButtons[i].clicked += () => OnQ20RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q21RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q21RatingButtons[i] != null)
                q21RatingButtons[i].clicked += () => OnQ21RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q22RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q22RatingButtons[i] != null)
                q22RatingButtons[i].clicked += () => OnQ22RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q23RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q23RatingButtons[i] != null)
                q23RatingButtons[i].clicked += () => OnQ23RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q24RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q24RatingButtons[i] != null)
                q24RatingButtons[i].clicked += () => OnQ24RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q25RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q25RatingButtons[i] != null)
                q25RatingButtons[i].clicked += () => OnQ25RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q26RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q26RatingButtons[i] != null)
                q26RatingButtons[i].clicked += () => OnQ26RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q27RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q27RatingButtons[i] != null)
                q27RatingButtons[i].clicked += () => OnQ27RatingButtonClick(index + 1);
        }
        for (int i = 0; i < q28RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q28RatingButtons[i] != null)
                q28RatingButtons[i].clicked += () => OnQ28RatingButtonClick(index + 1);
        }

        for (int i = 0; i < q29RatingButtons.Length; i++)
        {
            int index = i; // Local copy for the closure to avoid issues with lambda expressions
            if (q29RatingButtons[i] != null)
                q29RatingButtons[i].clicked += () => OnQ29RatingButtonClick(index + 1);
        }


    }
    private void OnQ11RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q11SelectedOption = rating.ToString();
    }
    private void OnQ12RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q12SelectedOption = rating.ToString();
    }
    private void OnQ13RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q13SelectedOption = rating.ToString();
    }
    private void OnQ14RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q14SelectedOption = rating.ToString();
    }
    private void OnQ15RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q15SelectedOption = rating.ToString();
    }
    private void OnQ16RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q16SelectedOption = rating.ToString();
    }
    private void OnQ17RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q17SelectedOption = rating.ToString();
    }
    private void OnQ18RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q18SelectedOption = rating.ToString();
    }
    private void OnQ19RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q19SelectedOption = rating.ToString();
    }
    private void OnQ20RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q20SelectedOption = rating.ToString();
    }
    private void OnQ21RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q21SelectedOption = rating.ToString();
    }
    private void OnQ22RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q22SelectedOption = rating.ToString();
    }
    private void OnQ23RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q23SelectedOption = rating.ToString();
    }
    private void OnQ24RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q24SelectedOption = rating.ToString();
    }
    private void OnQ25RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q25SelectedOption = rating.ToString();
    }
    private void OnQ26RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q26SelectedOption = rating.ToString();
    }
    private void OnQ27RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q27SelectedOption = rating.ToString();
    }
    private void OnQ28RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q28SelectedOption = rating.ToString();
    }
    private void OnQ29RatingButtonClick(int rating)
    {
        UserDataManager.Instance.Q29SelectedOption = rating.ToString();
    }



    // Update is called once per frame
    void Update()
{

}
}

