using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class HintMenuScript : MonoBehaviour
{
    public string currentHintText;
    public Button previousButtonObject = null;
    public TextMeshProUGUI hintText;
    public Text hintTextObject;
    Color hintNotSelected = new Color(1.0f,1.0f,1.0f,1.0f);
    Color hintSelected = new Color(.75f,.75f,.75f,1.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHintText(int newHintNumber)
    {
        switch (newHintNumber)
        {
            case 1: // How much time do I have? (Hint 1)
                currentHintText = "Take as much time as you need!";
                break;
            case 2: // How much money do I have? (Hint 2)
                currentHintText = "when life gives you lemons";
                break;
            case 3:
                currentHintText = "get mad, make life take the lemons back. I don't want your darn lemons, the heck am I supposed to do with these?";
                break;
            case 4:
                currentHintText = "Demand to see life's manager. Make life rue the day it thought it could give Cave Johnson lemons.";
                break;
            case 5:
                currentHintText = "I'm going to get my engineers to build a combustible lemon.";
                break;
            default:
                currentHintText = "bruh";
                break;
        }
        hintText.text = currentHintText;
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
    
}
