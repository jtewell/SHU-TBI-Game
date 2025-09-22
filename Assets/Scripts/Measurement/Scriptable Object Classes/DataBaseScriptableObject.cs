using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class DataBaseScriptableObject : ScriptableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetValueFields()
    {
        //Get all instance variables that are public
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        var fields = this.GetType().GetFields(flags);

        foreach (var field in fields)
        {
            //If the field is of type DataPairBase
            if (typeof(DataPair).IsAssignableFrom(field.FieldType))
            {
                //Cast it
                DataPair dataPair = (DataPair)field.GetValue(this);

                //Reset its value field
                dataPair.ResetValue();
            }
        }
    }

    public bool SetValueField (string fieldName, string newValue)
    {
        //Get all instance variables that are public
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        var field = this.GetType().GetField(fieldName, flags);

        //no field that matches that string
        if (field == null)
        {
            Debug.LogWarning($"No field named {fieldName}.");
            return false;
        }

        //Check if its a DataPairBase object (it should be)
        DataPair dataPair = field.GetValue(this) as DataPair;
        if (dataPair == null)
        {
            Debug.LogWarning($"Field {fieldName} is not of type DataPairBase.");
            return false;
        }

        //Get the value field
        var valueField = dataPair.GetType().GetField("value", flags);

        //Finally, perform the assignment
        valueField.SetValue(dataPair, newValue);
        return true;
    }

    public string GetValueField (string fieldName)
    {
        //Get all instance variables that are public
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        var field = this.GetType().GetField(fieldName, flags);

        //no field that matches that string
        if (field == null)
        {
            Debug.LogWarning($"No field named {fieldName}.");
            return null;
        }

        //Check if its a DataPairBase object (it should be)
        DataPair dataPair = field.GetValue(this) as DataPair;
        if (dataPair == null)
        {
            Debug.LogWarning($"Field {fieldName} is not of type DataPairBase.");
            return null;
        }

        //return the value found
        return dataPair.value;
    }

    public Dictionary<string, string> GetDataAsDictionary()
    {
        // Create a dictionary to hold the Google Form entry IDs and their corresponding values for all the data
        Dictionary<string, string> dictionaryData = new Dictionary<string, string> { };

        //Get all public fields of the scriptable object
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            //Check if its a DataPairBase object (it should be)
            DataPair dataPair = field.GetValue(this) as DataPair;
            if (dataPair == null)
            {
                Debug.LogWarning($"Field is not of type DataPairBase.");
                continue;
            }

            //Add the data pair to the dictionary
            dictionaryData[dataPair.entryID] = dataPair.value;
        }

        return dictionaryData;
    }
}

//Class used to store the data for each measurement (in string format) and their corresponding Google Form entry ID
[System.Serializable]
public class DataPair
{
    public string entryID;
    public string value;

    public DataPair (string entryID)
    {
        this.entryID = entryID;
    }

    public void ResetValue()
    {
        value = "";
    }
}