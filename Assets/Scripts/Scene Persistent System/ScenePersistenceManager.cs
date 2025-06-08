using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistenceManager : PersistentMonoSingleton<ScenePersistenceManager>
{
    private Dictionary<string, PersistentDataEntry> DataEntries = new Dictionary<string, PersistentDataEntry>();
    public void SetData<T>(string domain, string key, T data)
    {
        var lookup = $"{domain}|{key}";
        var obj = new PersistentDataEntry()
        {
            Type = typeof(T),
            Data = data
        };
        
        if(DataEntries.ContainsKey(lookup)) DataEntries[lookup] = obj;
        else DataEntries.Add(lookup, obj);
    }

    public T GetData<T>(string domain, string key, T defaultValue = default)
    {
        var lookup = $"{domain}|{key}";
        if (!DataEntries.ContainsKey(lookup))
        {
            return defaultValue;
        }
        var entry = DataEntries[lookup];
        
        if(!typeof(T).IsAssignableFrom(entry.Type))
        {
            Debugger.Break();
            throw new Exception("Type mismatch for data key: " + key + " stored as " + entry.Type);
            
        }
        return (T)entry.Data;
    }

    public bool IsOfType<T>(string domain, string key)
    {
        var lookup = $"{domain}|{key}";
        if (!DataEntries.ContainsKey(lookup))
        {
            throw new Exception("Data key not found: " + key);
        }
        var entry = DataEntries[lookup];

        return typeof(T).IsAssignableFrom(entry.Type);
    }

    public bool Contains(string domain, string key)
    {
        var lookup = $"{domain}|{key}";
        return (DataEntries.ContainsKey(lookup));

    }

    public void ClearDomain(string domain)
    {
        List<string> removalQueue = new List<string>(); 
        foreach (var entry in DataEntries.Keys)
        {
            if (entry.StartsWith($"{domain}|")) removalQueue.Add(entry);
        }
        foreach (var entry in removalQueue)
        {
            DataEntries.Remove(entry);
        }
    }

    public static string GetFullyQualifiedGameObjectName(GameObject gameObject)
    {
        string path = "/" + gameObject.name;
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            path = "/" + gameObject.name + path;
        }

        return $"{SceneManager.GetActiveScene().name}:{path}";
    }
    
    
}

public class PersistentDataEntry
{
    public Type Type;
    internal object Data;
    
}