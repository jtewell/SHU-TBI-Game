
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class UseDryerMachine : UnityEvent<string> { }

public class InteractableDryerMachine : MonoBehaviour
{
    public static UseDryerMachine useDryerMachine = new UseDryerMachine();

    public DryerMachineUIScript dryerMachineUI;



    public void Interact()
    {
        OpenDryerMachineUI();
    }

    public void OpenDryerMachineUI()
    {
        dryerMachineUI.OpenDryerMachineUI();
    }


}