using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class HintMenuScript : MonoBehaviour
{
    public Transform contentObject;
    public string currentHintText;
    public Button buttonPrefab;
    private int buttonArrayLength = 10;
    private Button[] buttonList = new Button[10]; // make sure the array length is exact
    Dictionary<int, (string question, string response)> buttonInfo = new Dictionary<int, (string question, string response)>();


    public Button previousButtonObject = null;
    public TextMeshProUGUI hintText;
    public Text hintTextObject;
    Color hintNotSelected = new Color(1.0f,1.0f,1.0f,1.0f);
    Color hintSelected = new Color(.75f,.75f,.75f,1.0f);
    // Start is called before the first frame update
    void Start()
    {
        SetButtonInfo();
        for(int i = 0; i<buttonList.Length; i++)
        {
            int index = i;
            Debug.Log(i);
            buttonList[i] = Instantiate(buttonPrefab);
            buttonList[i].transform.SetParent(this.gameObject.transform);
            buttonList[i].name = $"Button {i}";
            buttonList[i].GetComponentInChildren<TextMeshProUGUI>().text = buttonInfo[i].question;
            buttonList[i].GetComponent<Button>().onClick.AddListener(() => UpdateHintText(index));
            Debug.Log($"Button instantiated with listener parameter: {i}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHintText(int newHintNumber)
    {
        Debug.Log(newHintNumber);
        hintText.text = buttonInfo[newHintNumber].response;
        UpdateHintButtonColor(buttonList[newHintNumber]);
    }



    public void UpdateHintButtonColor(Button newHintButton)
    {
        if(previousButtonObject != null)
        {
            previousButtonObject.image.color = hintNotSelected;
        }
        newHintButton.image.color = hintSelected;
        previousButtonObject = newHintButton;
    }
    
    public void SetButtonInfo()
    {
        //var buttonInfo = new Dictionary<int, (string question, string response)>();
        // Button ID (first number) corresponds with the index of the button in the array.
        buttonInfo.Add(0, ("How much time do I have?", "Take as much time as you need!"));
        buttonInfo.Add(1, ("How much money do I have?", "when life gives you lemons"));
        buttonInfo.Add(2, ("Question 3", "get mad, make life take the lemons back. I don't want your darn lemons, the heck am I supposed to do with these?"));
        buttonInfo.Add(3, ("Question 3: The SQL", "Demand to see life's manager. Make life rue the day it thought it could give Cave Johnson lemons."));
        buttonInfo.Add(4, ("Question, the 5th", "I'm going to get my engineers to build a combustible lemon."));
        buttonInfo.Add(5, ("Question 6", "Answer 6"));
        buttonInfo.Add(6, ("Question 7 (don't say it)", "It really isn't that funny"));
        buttonInfo.Add(7, ("Question 8", "Answer 8 (I'm running out of ideas)"));
        buttonInfo.Add(8, ("Question, the 5th", "I'm going to get my engineers to build a combustible lemon."));
        buttonInfo.Add(9, ("Question, the 5th", "I'm going to get my engineers to build a combustible lemon."));
        
    }
}
