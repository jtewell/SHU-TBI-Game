
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class UseWashingMachine : UnityEvent<string> { }

public class InteractableWasherMachine : MonoBehaviour
{
    public static UseWashingMachine useWashingMachine = new UseWashingMachine();

    public WashingMachineUI washingMachineUI;

    

    public void Interact()
    {
        Debug.Log("Opening washer UI");
        OpenWashingMachineUI();
    }

    public void OpenWashingMachineUI()
    {
        washingMachineUI.OpenWashingMachineUI();
    }


    

}
