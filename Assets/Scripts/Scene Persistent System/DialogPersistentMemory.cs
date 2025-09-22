
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogPersistentMemory : Yarn.Unity.VariableStorageBehaviour
{

    public const string DOMAIN = "YARN_SPINNER";
    // main function used by Dialogue Runner to retrieve Yarn variable values
    public override bool TryGetValue<T>(string variableName, out T result) {
        // retrieve value with key "variableName" from a list or dictionary, etc.
        if (!ScenePersistenceManager.Instance.Contains(DOMAIN, variableName))
        {
            result = default(T);
            return false;
        }

        if (ScenePersistenceManager.Instance.IsOfType<T>(DOMAIN, variableName) == false)
        {
            Debug.LogError("Yarn Variable " + variableName + " is not of type " + typeof(T));
            throw new InvalidOperationException("Yarn Variable " + variableName + " is not of type " + typeof(T) + "");
        }

        result = ScenePersistenceManager.Instance.GetData<T>(DOMAIN, variableName);
        return true;
    }
    
    // overload for setting a String variable
    public override void SetValue(string variableName, string stringValue) {
        ScenePersistenceManager.Instance.SetData(DOMAIN, variableName, stringValue);
    }
    
    // overload for setting a Float variable
    public override void SetValue(string variableName, float floatValue) {
        ScenePersistenceManager.Instance.SetData(DOMAIN, variableName, floatValue);
    }
    
    // overload for setting a Boolean variable
    public override void SetValue(string variableName, bool boolValue) {
        ScenePersistenceManager.Instance.SetData(DOMAIN, variableName, boolValue);
    }

    // clear all variable data        
    public override void Clear() {
        Debug.Log("Clearing all variables for Yarn");
        ScenePersistenceManager.Instance.ClearDomain(DOMAIN);
        
    }

    public override bool Contains(string variableName)
    {
        return ScenePersistenceManager.Instance.Contains(DOMAIN, variableName);
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        throw new System.NotImplementedException();
    }

    public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
    {
        throw new System.NotImplementedException();
    }
}
