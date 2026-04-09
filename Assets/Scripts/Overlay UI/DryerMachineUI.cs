using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DryerMachineUI : MonoBehaviour
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
    public GameObject[] cycleIndicators; // 0 = 20 min, 1 = 40 min, 2 = 60 min

    [SerializeField]
    private InteractableDryerMachine dryerMachine;

    public GameObject DryerMachineUIPanel;

    [Header("Dryer Cycling")]
    public Sprite[] Sprites;
    public Sprite MachineOpen;
    public Sprite MachineOpenDone;

    public float FrameInterval = 0.2f;

    [Header("Cycle Duration")]
    public float CycleDuration = 0f;

    [Header("Completion")]
    public Sprite FinishedSprite;

    private DryerMachineUI dryerMachineUI;

    [Header("Targets")]
    public Image TargetImage;
    public SpriteRenderer TargetRenderer;

    private int _currentIndex;
    private Coroutine _cycleCoroutine;
    private bool _stopRequested;

    private bool tempButtonPressed = false;
    private bool cycleButtonPressed = false;

    private void OnEnable()
    {
        if (twentyButton != null)
        {
            twentyButton.onClick.AddListener(() => OnCycleButtonPressed(0));
        }
        if (fortyButton != null)
        {
            fortyButton.onClick.AddListener(() => OnCycleButtonPressed(1));
        }
        if (sixtyButton != null)
        {
            sixtyButton.onClick.AddListener(() => OnCycleButtonPressed(2));
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


        if (cycleIndicators != null && cycleIndicators.Length >= 3)
        {
            // Initialize all indicators to inactive
            foreach (var indicator in cycleIndicators)
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

        // If dryerMachine not assigned in Inspector, try to find one on this GameObject or its parents.
        if (dryerMachine == null)
        {
            dryerMachine = GetComponentInParent<InteractableDryerMachine>();
        }

        if (runButton == null)
        {
            Debug.LogWarning($"Run Button is not assigned on {name}. Please assign the UI Button in the Inspector.", this);
        }

        if (dryerMachine == null)
        {
            Debug.LogWarning($"InteractableDryerMachine reference is not assigned and couldn't be found in parents of {name}. Run action will not work until assigned.", this);
        }

        if (DryerMachineUIPanel != null)
        {
            DryerMachineUIPanel.SetActive(false);


            if (dryerMachineUI == null)
            {
                dryerMachineUI = DryerMachineUIPanel.GetComponentInChildren<DryerMachineUI>();
            }
        }

        // Ensure sane defaults
        FrameInterval = Mathf.Max(0.02f, FrameInterval);
        CycleDuration = Mathf.Max(0.02f, CycleDuration);
    }

    public void OpenDryerMachineUI()
    {
        if (DryerMachineUIPanel != null)
        {
            DryerMachineUIPanel.SetActive(true);
            TargetImage.sprite = FinishedSprite;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
        if (tempButtonPressed)
        {
            // Request stop of any existing run and ensure coroutine is stopped
            _stopRequested = true;
            if (_cycleCoroutine != null)
            {
                StopCoroutine(_cycleCoroutine);
                _cycleCoroutine = null;
            }
            

            if (CycleDuration != 0f)
            {
                //If any time button was pressed, reset cancellation flag and start new cycle
                _stopRequested = false;
                _currentIndex = 0;
                _cycleCoroutine = StartCoroutine(CycleSprites());
            }
        }
    }

    private void OnOpenButtonPressed()
    {
        TargetImage.sprite = MachineOpen;
        InitializeCloseButton();
    }
    private void OnCloseButtonPressed()
    {
        TargetImage.sprite = Sprites[0];
        InitializeRunButton();
    }

    private IEnumerator CycleSprites()
    {
        if (Sprites == null || Sprites.Length == 0)
        {
            _cycleCoroutine = null;
            yield break;
        }

        // Ensure a sane per-frame interval
        float interval = Mathf.Max(0.02f, FrameInterval);

        // Compute end time for the total cycle duration
        float endTime = Time.time + CycleDuration;

        // Loop until time runs out, UI is closed, or a stop is requested
        while (!_stopRequested && (DryerMachineUIPanel == null || DryerMachineUIPanel.activeSelf) && Time.time < endTime)
        {
            // Clamp index just in case
            if (_currentIndex < 0 || _currentIndex >= Sprites.Length)
            {
                _currentIndex = 0;
            }

            // Apply current sprite to the assigned target
            Sprite current = Sprites[_currentIndex];
            if (TargetImage != null)
            {
                TargetImage.sprite = current;
            }
            else if (TargetRenderer != null)
            {
                TargetRenderer.sprite = current;
            }

            // Advance to next frame (wrap to loop)
            _currentIndex = (_currentIndex + 1) % Sprites.Length;

            // Wait for the configured interval, but allow early exit if stop requested by checking after wait
            yield return new WaitForSeconds(interval);
        }

        // If the cycle wasn't explicitly stopped early, show the finished sprite (or last frame)
        if (!_stopRequested)
        {
            Sprite finished = FinishedSprite != null ? FinishedSprite : Sprites[Sprites.Length - 1];

            if (TargetImage != null)
            {
                TargetImage.sprite = finished;
            }
            else if (TargetRenderer != null)
            {
                TargetRenderer.sprite = finished;
            }
        }

        // Mark coroutine as finished
        _cycleCoroutine = null;
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

    private void UpdateCycleIndicators(int cycle)
    {
        if (cycleIndicators == null || cycleIndicators.Length < 3)
        {
            return;
        }

        for (int i = 0; i < cycleIndicators.Length; i++)
        {
            var indicator = cycleIndicators[i];
            if (indicator == null)
            {
                continue;
            }

            indicator.SetActive(i == cycle);
        }
    }

    public void OnTempButtonPressed(int temp)
    {
        // Always update the UI indicators so user sees immediate feedback
        UpdateTempIndicators(temp);
        tempButtonPressed = true;

    }

    public void OnCycleButtonPressed(int cycle)
    {
        UpdateCycleIndicators(cycle);
        if (cycle == 0)
        {
            CycleDuration = 5f;
        }
        else if (cycle == 1)
        {
            CycleDuration = 10f;
        }
        else if (cycle == 2)
        {
            CycleDuration = 15f;
        }
    }

    private void OnExitButtonPressed()
    {
        DryerMachineUIPanel.SetActive(false);

    }
}