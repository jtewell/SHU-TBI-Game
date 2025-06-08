using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DisappearOnceSeenAndNeverComeBack : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        if (ScenePersistenceManager.Instance.GetData<bool>("seeOnce",
                         ScenePersistenceManager.GetFullyQualifiedGameObjectName(gameObject), false))
        {
                     gameObject.SetActive(false);
        }    
    }
    
    public void MarkAsSeen()
    {
        ScenePersistenceManager.Instance.SetData("seeOnce", ScenePersistenceManager.GetFullyQualifiedGameObjectName(gameObject), true);
    }

    public static void ResetAll()
    {
        ScenePersistenceManager.Instance.ClearDomain("seeOnce");
    }
}
