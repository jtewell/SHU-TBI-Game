using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class MeasurementDataManager : PersistentMonoSingleton<MeasurementDataManager>
{
    [Header("The Session Data scriptable object")]
    [SerializeField] SessionData sessionData;

    [Header("The Google form URL to send the data to")]
    // Set the Google Apps Script web app URL here (make sure it's published to the web)
    [SerializeField] private string url = "https://docs.google.com/forms/d/e/1FAIpQLSe1CpGcYz-zV-OV8xfAl0BP4K8g2WQtLwgLaMbge6YOgMrrfw/formResponse";

    [Header("Toggle this on to send the data to the form, off to not send the data")]
    [SerializeField] private bool sendToGoogleForm = true;

    /* Questionaire Variables
    * These collect data from the questionaire at the start of the game
    * At the end of the questionaire, this data is pushed into the questionaire data scriptable object
    */
    [Header("Questionaire Variables")]
    public string userID;
    public string birthMonth;
    public string birthDay;
    public string birthYear;
    public string gender;
    public string Q3SelectedOption; // hasTBIHistory;
    public string Q4SelectedOption; // hadConcussions;
    public string Q5SelectedOption; // numberOfConcussions;
    public string Q6SelectedOption; // yearLastConcussion;
    public string Q7SelectedOption; // hasTBI;
    public string Q8SelectedOption; // numberOfTBIs;
    public string Q9SelectedOption; // yearLastTBI;
    public string Q11SelectedOption; // canCommunicateWell;
    public string Q12SelectedOption; // canKeepUpConversations;
    public string Q13SelectedOption; // canRemeberConversations;
    public string Q14SelectedOption; // canRememberSteps;
    public string Q15SelectedOption; // canFollowInstructions;
    public string Q16SelectedOption; // canFollowLocationInstructions;
    public string Q17SelectedOption; // getLostEasily;
    public string Q18SelectedOption; // getConfusedPeopleTalk;
    public string Q19SelectedOption; // forgetItemsToPurchase;
    public string Q20SelectedOption; // canFocusOnTask;
    public string Q21SelectedOption; // canChangePlans;
    public string Q22SelectedOption; // canModifyPlans;
    public string Q23SelectedOption; // canRememberDetails;
    public string Q24SelectedOption; // canLearnTasks;
    public string Q25SelectedOption; // makeSenseTalking;
    public string Q26SelectedOption; // rambleOffTopic;
    public string Q27SelectedOption; // canGoBackOnTopic;
    public string Q28SelectedOption; // canManageMoney;
    public string Q29SelectedOption; // alwaysUseShoppingList;

    /* Measurement Variables
    * These collect data from various game objects in a narrative (i.e.player, items, NPCs)
    * At the end of a narrative, this data is pushed into the narrative data scriptable object
    * Then this data is reset for the next loaded narrative
    */
    [Header ("Current Narrative Measurement Variables")]
    public float distanceTraveled;
    public float timeTaken;
    public int numberTimesStopped;

    public int numberTimesMapOpened;
    public int numberTimesInventoryOpened;
    public int numberTimesToDoListOpened;

    public float timeMapOpened;
    public float timeInventoryOpened;
    public float timeToDoListOpened;

    public string locationsVisited;
    public string itemsInteracted;
    public string peopleInteracted;

    // Start is called before the first frame update
    public void Start()
    {
        InitializeSessionData();
    }

    private void SubmitDataToGoogleForm()
    {
        //SaveQuestionaireData();

        if (sendToGoogleForm)
        {
            StartCoroutine(SubmitDataCoroutine());
        }
        
    }

    // Load next Narrative
    public void LoadNextNarrative ()
    {
        //Save all current measurements to
        sessionData.loadNextNarrativeScriptableObject();
    }

    //Save the questionaire data to the session data
    public void SaveQuestionaireData ()
    {
        sessionData.SetValueFieldQuestionaire("userID", userID);
        sessionData.SetValueFieldQuestionaire("birthMonth", birthMonth);
        sessionData.SetValueFieldQuestionaire("birthDay", birthDay);
        sessionData.SetValueFieldQuestionaire("birthYear", birthYear);
        sessionData.SetValueFieldQuestionaire("gender", gender);
        sessionData.SetValueFieldQuestionaire("hasTBIHistory", Q3SelectedOption);
        sessionData.SetValueFieldQuestionaire("hadConcussions", Q4SelectedOption);
        sessionData.SetValueFieldQuestionaire("numberOfConcussions", Q5SelectedOption);
        sessionData.SetValueFieldQuestionaire("yearLastConcussion", Q6SelectedOption);
        sessionData.SetValueFieldQuestionaire("hasTBI", Q7SelectedOption);
        sessionData.SetValueFieldQuestionaire("numberOfTBIs", Q8SelectedOption);
        sessionData.SetValueFieldQuestionaire("yearLastTBI", Q9SelectedOption);
        sessionData.SetValueFieldQuestionaire("canCommunicateWell", Q11SelectedOption);
        sessionData.SetValueFieldQuestionaire("canKeepUpConversations", Q12SelectedOption);
        sessionData.SetValueFieldQuestionaire("canRememberConversations", Q13SelectedOption);
        sessionData.SetValueFieldQuestionaire("canRememberSteps", Q14SelectedOption);
        sessionData.SetValueFieldQuestionaire("canFollowInstructions", Q15SelectedOption);
        sessionData.SetValueFieldQuestionaire("canFollowLocationInstructions", Q16SelectedOption);
        sessionData.SetValueFieldQuestionaire("getLostEasily", Q17SelectedOption);
        sessionData.SetValueFieldQuestionaire("getConfusedPeopleTalk", Q18SelectedOption);
        sessionData.SetValueFieldQuestionaire("forgetItemsToPurchase", Q19SelectedOption);
        sessionData.SetValueFieldQuestionaire("canFocusOnTask", Q20SelectedOption);
        sessionData.SetValueFieldQuestionaire("canChangePlans", Q21SelectedOption);
        sessionData.SetValueFieldQuestionaire("canModifyPlans", Q22SelectedOption);
        sessionData.SetValueFieldQuestionaire("canRememberDetails", Q23SelectedOption);
        sessionData.SetValueFieldQuestionaire("canLearnTasks", Q24SelectedOption);
        sessionData.SetValueFieldQuestionaire("makeSenseTalking", Q25SelectedOption);
        sessionData.SetValueFieldQuestionaire("rambleOffTopic", Q26SelectedOption);
        sessionData.SetValueFieldQuestionaire("canGoBackOnTopic", Q27SelectedOption);
        sessionData.SetValueFieldQuestionaire("canManageMoney", Q28SelectedOption);
        sessionData.SetValueFieldQuestionaire("alwaysUseShoppingList", Q29SelectedOption);
    }

    private void OnEnable()
    {
        QuestManager.onFinishQuest.AddListener(SaveNarrativeData);
        QuestManager.onFinishAllQuests.AddListener(SubmitDataToGoogleForm);
    }

    private void OnDisable()
    {
        QuestManager.onFinishQuest.RemoveListener(SaveNarrativeData);
        QuestManager.onFinishAllQuests.RemoveListener(SubmitDataToGoogleForm);
    }

    private void SaveNarrativeData ()
    {
        sessionData.SetValueFieldNarrative ("distanceTraveled", distanceTraveled.ToString());
        sessionData.SetValueFieldNarrative("timeTaken", timeTaken.ToString());
        sessionData.SetValueFieldNarrative("numberTimesStopped", numberTimesStopped.ToString());

        sessionData.SetValueFieldNarrative("numberTimesMapOpened", numberTimesMapOpened.ToString());
        sessionData.SetValueFieldNarrative("numberTimesInventoryOpened", numberTimesInventoryOpened.ToString());
        sessionData.SetValueFieldNarrative("numberTimesToDoOpened", numberTimesToDoListOpened.ToString());

        sessionData.SetValueFieldNarrative("timeMapOpened", timeMapOpened.ToString());
        sessionData.SetValueFieldNarrative("timeInventoryOpened", timeInventoryOpened.ToString());
        sessionData.SetValueFieldNarrative("timeToDoListOpened", timeToDoListOpened.ToString());

        sessionData.SetValueFieldNarrative("numberDoorsInteracted", locationsVisited);
        sessionData.SetValueFieldNarrative("numberItemsInteracted", itemsInteracted);
        sessionData.SetValueFieldNarrative("numberNPCSInteracted", peopleInteracted);
    }

    private void InitializeSessionData()
    {
        //Initialize all narrative measurements to their default values (no need to do this for questionaire)
        DeleteMeasurements();
        //Go though all the scriptable object data and make sure they are also wiped (except for the Google Form entry IDs)
        sessionData.ResetSessionData();
        //Generate a new user ID for the new game session
        GenerateUniqueId();
    }

    //Internally called to delete all the current measurements (used when loading the next narrative)
    private void DeleteMeasurements()
    {
        distanceTraveled = 0;
        timeTaken = 0;
        numberTimesStopped = 0;

        numberTimesMapOpened = 0;
        numberTimesInventoryOpened = 0;
        numberTimesToDoListOpened = 0;

        timeMapOpened = 0;
        timeInventoryOpened = 0;
        timeToDoListOpened = 0;

        locationsVisited = "";
        itemsInteracted = "";
        peopleInteracted = "";
    }

    // Method to generate a random unique identifier
    private void GenerateUniqueId()
    {
        // Generate a random number and store it as the user ID
        userID = Random.Range(100000, 999999).ToString();
    }

    private IEnumerator SubmitDataCoroutine()
    {
        // Create a dictionary to hold the data from all session data scriptable objects, start with the questioniare data
        Dictionary<string, string> formDataDictionary = sessionData.questionaireData.GetDataAsDictionary();

        //Go through the list of all narrative data and append them to the dictionary
        foreach (NarrativeData scriptableObject in sessionData.narratives)
        {
            formDataDictionary = formDataDictionary.Concat(scriptableObject.GetDataAsDictionary()).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        //Store the dictionary data in a WWWForm
        WWWForm form = new WWWForm();

        // Iterate over the dictionary and add each field to the form
        foreach (KeyValuePair<string, string> field in formDataDictionary)
        {
            form.AddField(field.Key, field.Value);
        }

        // Create the UnityWebRequest for a POST request
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("FeedBack Submitted Successfully");
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        }
    }
}