using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTriggerBoxEvent : MonoBehaviour
{
    [Header("Trigger Enter Event Section")]
    public bool enterIsOneShot;
    public float enterEventCooldown;
    public UnityEvent onTriggerEnterEvent;
    
    [Space]
    [Header("Trigger Exit Event Section")]
    public bool exitIsOneShot;
    public float exitEventCooldown;
    public UnityEvent onTriggerExitEvent;
    
    private bool _enterHasBeenTriggered;
    private float _enterTimer;
    
    private bool _exitHasBeenTriggered;
    private float _exitTimer;

    void Start()
    {
        _enterTimer = enterEventCooldown;
        _exitTimer = exitEventCooldown;
    }

    void OnTriggerEnter(Collider other)
    {
        if(enterIsOneShot && _enterHasBeenTriggered)
            return;

        if(enterEventCooldown > _enterTimer)
            return;

        onTriggerEnterEvent.Invoke();
        _enterHasBeenTriggered = true;
        _enterTimer = 0f;
    }
    
    void OnTriggerExit(Collider other)
    {
        if(exitIsOneShot && _exitHasBeenTriggered)
            return;

        if(exitEventCooldown > _exitTimer)
            return;

        onTriggerEnterEvent.Invoke();
        _exitHasBeenTriggered = true;
        _exitTimer = 0f;
    }

    void Update()
    {
        if (_enterHasBeenTriggered)
            _enterTimer += Time.deltaTime;
        
        if (_exitHasBeenTriggered)
            _exitTimer += Time.deltaTime;
    }
}