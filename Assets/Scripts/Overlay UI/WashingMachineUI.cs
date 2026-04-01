using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WashingMachineUI : MonoBehaviour
{
    public Button runButton;
    public Button twentyButton;
    public Button fortyButton;
    public Button sixtyButton;
    public Button exitButton;
    public Button openButton;
    public Button closeButton;
    public Button[] tempButtons; // 0 = cold, 1 = warm, 2 = hot
    public GameObject[] tempIndicators; // 0 = cold, 1 = warm, 2 = hot

    [SerializeField]
    private InteractableWasherMachine washerMachine;


    private void OnEnable()
    {
        if (twentyButton != null)
        {
            twentyButton.onClick.AddListener(OnTwentyButtonPressed);
        }
        if (fortyButton != null)
        {
            fortyButton.onClick.AddListener(OnFortyButtonPressed);
        }
        if (sixtyButton != null)
        {
            sixtyButton.onClick.AddListener(OnSixtyButtonPressed);
        }
        if (openButton != null)
        {
            openButton.onClick.AddListener(OnOpenButtonPressed);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonPressed);
        }
        if (tempButtons != null && tempButtons.Length >= 3)
        {
            // Remove existing listeners first in case OnEnable was called multiple times
            tempButtons[0].onClick.RemoveAllListeners();
            tempButtons[1].onClick.RemoveAllListeners();
            tempButtons[2].onClick.RemoveAllListeners();

            tempButtons[0].onClick.AddListener(() => OnTempButtonPressed(0)); // Cold
            tempButtons[1].onClick.AddListener(() => OnTempButtonPressed(1)); // Warm
            tempButtons[2].onClick.AddListener(() => OnTempButtonPressed(2)); // Hot
        }
        if (tempIndicators != null && tempIndicators.Length >= 3)
        {
            // Initialize all indicators to inactive
            foreach (var indicator in tempIndicators)
            {
                if (indicator != null)
                {
                    indicator.SetActive(false);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (twentyButton != null)
        {
            twentyButton.onClick.RemoveListener(OnTwentyButtonPressed);
        }
        if (fortyButton != null)
        {
            fortyButton.onClick.RemoveListener(OnFortyButtonPressed);
        }
        if (sixtyButton != null)
        {
            sixtyButton.onClick.RemoveListener(OnSixtyButtonPressed);
        }
        if (openButton != null)
        {
            openButton.onClick.RemoveListener(OnOpenButtonPressed);
        }
        if (tempButtons != null && tempButtons.Length >= 3)
        {
            tempButtons[0].onClick.RemoveAllListeners();
            tempButtons[1].onClick.RemoveAllListeners();
            tempButtons[2].onClick.RemoveAllListeners();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {

        // If washerMachine not assigned in Inspector, try to find one on this GameObject or its parents.
        if (washerMachine == null)
        {
            washerMachine = GetComponentInParent<InteractableWasherMachine>();
        }

        if (runButton == null)
        {
            Debug.LogWarning($"Run Button is not assigned on {name}. Please assign the UI Button in the Inspector.", this);
        }

        if (washerMachine == null)
        {
            Debug.LogWarning($"InteractableWasherMachine reference is not assigned and couldn't be found in parents of {name}. Run action will not work until assigned.", this);
        }
    }

    public void InitializeRunButton()
    {
        runButton.onClick.AddListener(OnRunButtonPressed);
    }

    public void InitializeCloseButton()
    {
        closeButton.onClick.AddListener(OnCloseButtonPressed);
    }

    public void InitializeTempButtons()
    {
        if (tempButtons != null && tempButtons.Length >= 3)
        {
            tempButtons[0].onClick.AddListener(() => OnTempButtonPressed(0)); // Cold
            tempButtons[1].onClick.AddListener(() => OnTempButtonPressed(1)); // Warm
            tempButtons[2].onClick.AddListener(() => OnTempButtonPressed(2)); // Hot
        }
        else
        {
            Debug.LogWarning($"Temp buttons are not properly assigned on {name}. Please assign 3 buttons for cold, warm, and hot in the Inspector.", this);
        }
    }

    private void OnRunButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.OnRunButtonHit();
            return;
        }

        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnRunButtonHit();
            return;
        }

        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnRunButtonHit();
            return;
        }

        Debug.LogWarning($"Run button pressed but no InteractableWasherMachine found for {name}.", this);
    }

    private void OnTwentyButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.OnTwentyButtonHit();
            return;
        }

        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnTwentyButtonHit();
            return;
        }

        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnTwentyButtonHit();
            return;
        }
    }
    private void OnFortyButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.OnFortyButtonHit();
            return;
        }

        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnFortyButtonHit();
            return;
        }

        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnFortyButtonHit();
            return;
        }
    }
    private void OnSixtyButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.OnSixtyButtonHit();
            return;
        }

        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnSixtyButtonHit();
            return;
        }

        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnSixtyButtonHit();
            return;
        }
    }
    private void OnOpenButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.OnOpenButtonHit();
            return;
        }
        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnOpenButtonHit();
            return;
        }
        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnOpenButtonHit();
            return;
        }
        closeButton.onClick.AddListener(OnCloseButtonPressed);
    }
    private void OnCloseButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.OnCloseButtonHit();
            return;
        }
        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnCloseButtonHit();
            return;
        }
        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnCloseButtonHit();
            return;
        }
        InitializeRunButton();
    }

    // Helper to update UI indicators safely
    private void UpdateTempIndicators(int temp)
    {
        if (tempIndicators == null || tempIndicators.Length < 3)
        {
            return;
        }

        for (int i = 0; i < tempIndicators.Length; i++)
        {
            var indicator = tempIndicators[i];
            if (indicator == null)
            {
                continue;
            }

            indicator.SetActive(i == temp);
        }
    }

    public void OnTempButtonPressed(int temp)
    {
        // Always update the UI indicators so user sees immediate feedback
        UpdateTempIndicators(temp);

        // Then notify the washer machine (use existing fallbacks)
        if (washerMachine != null)
        {
            washerMachine.OnTempButtonHit();
            return;
        }
        // Fallback: try to find one dynamically if not assigned
        var found = GetComponentInParent<InteractableWasherMachine>();
        if (found != null)
        {
            found.OnTempButtonHit();
            return;
        }
        // Additional fallback: search scene for a washer machine that references this panel
        var associated = FindAssociatedWasherMachine();
        if (associated != null)
        {
            associated.OnTempButtonHit();
            return;
        }
    }
    private void OnExitButtonPressed()
    {
        if (washerMachine != null)
        {
            washerMachine.ExitWashingMachineUI();
        }
        
    }
    // Search the scene for an InteractableWasherMachine that references this UI panel (or contains it in the hierarchy).
    private InteractableWasherMachine FindAssociatedWasherMachine()
    {
        var all = FindObjectsOfType<InteractableWasherMachine>();
        foreach (var wm in all)
        {
            if (wm == null)
            {
                continue;
            }

            var panel = wm.WashingMachineUIPanel;
            if (panel == null)
            {
                continue;
            }

            // Direct match
            if (panel == gameObject)
            {
                return wm;
            }

            // Panel is a parent of this UI component
            if (transform.IsChildOf(panel.transform))
            {
                return wm;
            }

            // This UI component is the parent (or ancestor) of the panel
            if (panel.transform.IsChildOf(transform))
            {
                return wm;
            }
        }

        return null;
    }
}



