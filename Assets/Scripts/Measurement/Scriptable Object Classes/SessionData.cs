using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Session Data", menuName = "Measurements/Session Data", order = 1)]
public class SessionData : ScriptableObject
{
    //Holds the data from the questionaire
    public QuestionaireData questionaireData;

    //List to be populated with all the NarrativeData scriptable objects for the session
    public List <NarrativeData> narratives = new List<NarrativeData> ();

    //Keeps track of which narrative the game is on
    // 0 - Tutorial
    // 1 - Sandwhich Shop
    private int currentNarrative = 0;

    public void ResetSessionData ()
    {
        //Reset all value fields if they have not yet been erase still (should happen when the game ends)
        questionaireData.ResetValueFields();

        foreach (NarrativeData narrativeData in narratives)
        {
            narrativeData.ResetValueFields();
        }
    }

    public void SetValueFieldQuestionaire (string field, string value)
    {
        questionaireData.SetValueField(field, value);
    }

    public void SetValueFieldNarrative(string field, string value)
    {
        narratives[currentNarrative].SetValueField(field, value);
    }

    public void loadNextNarrativeScriptableObject()
    {
        currentNarrative++;
    }
}