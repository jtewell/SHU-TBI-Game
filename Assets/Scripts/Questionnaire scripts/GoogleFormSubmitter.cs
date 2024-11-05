using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GoogleFormSubmitter : MonoBehaviour
{
    // Singleton instance of the GoogleFormSubmitter class
    public static GoogleFormSubmitter Instance { get; private set; }


    // Set your Google Apps Script web app URL here (make sure it's published to the web)
    [SerializeField] // This makes a private field visible in the Inspector
    private string url = "https://docs.google.com/forms/d/e/1FAIpQLSe1CpGcYz-zV-OV8xfAl0BP4K8g2WQtLwgLaMbge6YOgMrrfw/formResponse";

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SubmitDataToSheet(Dictionary<string, string> formData)
    {
        StartCoroutine(SubmitDataCoroutine(formData));
    }
    private IEnumerator SubmitDataCoroutine(Dictionary<string, string> formData)
    {
        WWWForm form = new WWWForm();

        // Iterate over the dictionary and add each field to the form
        foreach (KeyValuePair<string, string> field in formData)
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
