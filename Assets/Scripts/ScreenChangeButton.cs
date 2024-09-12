using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ScreenChangeButton : MonoBehaviour
{
    // Declare the buttons
    private Button AcceptButton;
    

    // Start is called before the first frame update
    void Start()
    {
        // Access the UI Document and retrieve the root visual element
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the button with the name "AcceptButton"
        AcceptButton = root.Q<Button>("AcceptButton");

        // Add a click event to the "AcceptButton" to load the next scene
        if (AcceptButton != null)
        {
            AcceptButton.clicked += () =>
            {
                Debug.Log("AcceptButton Found");
                // Load the next scene
                SceneManager.LoadScene("ScreeningQuestions");
            };
        }
        else
        {
            Debug.Log("AcceptButton not found");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
