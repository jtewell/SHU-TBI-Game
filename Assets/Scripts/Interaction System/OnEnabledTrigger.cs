using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnabledTrigger : MonoBehaviour
{
    [Header("Trigger Event Section")]
    public UnityEvent onTriggerEvent;
    public bool triggerIsOneShot = false;

    private bool _hasBeenTriggered = false;

    public void onTrigger ()
    {
        //Ignore if object is disabled
        if (gameObject.activeInHierarchy == false) return;

        //Ignore more than one event call
        if (triggerIsOneShot == true && _hasBeenTriggered == true) return;

        //Mark triggered in one shot is true
        if (triggerIsOneShot == true)
        {
            _hasBeenTriggered = true;
        }

        //As long as this object is enabled, trigger any callbacks
        onTriggerEvent.Invoke();
    }
}
