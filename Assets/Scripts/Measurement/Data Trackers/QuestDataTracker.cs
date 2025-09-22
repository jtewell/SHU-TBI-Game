using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataTracker : MonoBehaviour
{
    private float startQuestTime;
    private float endQuestTime;

    public void DataUpdateQuestComplete ()
    {
        endQuestTime = Time.time;
        float questCompletedTime = endQuestTime - startQuestTime;
        MeasurementDataManager.Instance.timeTaken = questCompletedTime;
    }

    public void RestartQuestTimer()
    {
        startQuestTime = Time.time;
    }
}
