using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DryerMachineUIScript : MonoBehaviour
{
    public Button runButton;
    public Button twentyButton;
    public Button fortyButton;
    public Button sixtyButton;
    public Button exitButton;
    public Button openButton;
    public Button closeButton;
    public Button[] tempButtons; // 0 = cold, 1 = warm, 2 = hot
    public Button[] SoilButtons; // 0 = Low, 1 = Med, 2 = High
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI StatusText;

    [SerializeField]
    private InteractableDryerMachine dryerMachine;

    public GameObject DryerMachineUIPanel;

    [Header("Dryer Cycling")]
    public Sprite[] Sprites;
    public Sprite MachineOpen;
    public Sprite MachineOpenDone;

    public float FrameInterval = 0.2f;

    [Header("Cycle Duration")]
    public float CycleDuration = 10f;

    [Header("Completion")]
    public Sprite FinishedSprite;

    private DryerMachineUIScript dryerMachineUI;

    [Header("Targets")]
    public Image TargetImage;
    public SpriteRenderer TargetRenderer;

    private int _currentIndex;
    private Coroutine _cycleCoroutine;
    private bool _stopRequested;

    private bool readyToRun = false;

    // Countdown state
    private Coroutine _countdownCoroutine;
    private float _timeRemaining;
    
    private bool machineOpen = false;

    private void OnEnable()
    {
        if (openButton != null)
        {
            openButton.onClick.AddListener(OnOpenButtonPressed);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonPressed);
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
                dryerMachineUI = DryerMachineUIPanel.GetComponentInChildren<DryerMachineUIScript>();
            }
        }

        // Ensure sane defaults
        FrameInterval = Mathf.Max(0.02f, FrameInterval);
        CycleDuration = Mathf.Max(0.02f, CycleDuration);

        if (StatusText != null)
        {
            StatusText.text = "Idle";
        }

        // Ensure buttons are interactable initially
        EnableAllButtons();
    }

    public void OpenDryerMachineUI()
    {
        if (DryerMachineUIPanel != null)
        {
            DryerMachineUIPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // re-enable buttons when opening the UI
            EnableAllButtons();
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

    private void OnRunButtonPressed()
    {
        if (readyToRun)
        {
            // Stop any existing countdown so we don't run multiples
            StopCountdown();

            // Request stop of any existing run and ensure coroutine is stopped
            _stopRequested = true;
            if (_cycleCoroutine != null)
            {
                StopCoroutine(_cycleCoroutine);
                _cycleCoroutine = null;
            }

            _timeRemaining = 10f;
            if (timeText != null)
            {
                timeText.text = $"{Mathf.CeilToInt(_timeRemaining)}s";
            }

            CycleDuration = _timeRemaining;

            // Start countdown (single time-based coroutine)
            _countdownCoroutine = StartCoroutine(CountdownCoroutine());

            StatusText.text = "Running";

            _stopRequested = false;
            _currentIndex = 0;
            _cycleCoroutine = StartCoroutine(CycleSprites());
        }
    }

    private void OnOpenButtonPressed()
    {
        if (machineOpen == false)
        {
            TargetImage.sprite = MachineOpen;
            machineOpen = true;
        }   
        if (machineOpen)
        {
            TargetImage.sprite = MachineOpenDone;
            InitializeCloseButton();
        }
    }
    private void OnCloseButtonPressed()
    {
        if (Sprites != null && Sprites.Length > 0)
        {
            TargetImage.sprite = Sprites[0];
        }
        InitializeRunButton();
        if (StatusText != null)
        {
            StatusText.text = "Pick Temp";
        }
        if (tempButtons != null)
        {
            tempButtons[0].onClick.AddListener(OnTempButtonPressed);
            tempButtons[1].onClick.AddListener(OnTempButtonPressed);
            tempButtons[2].onClick.AddListener(OnTempButtonPressed);
        }

        // stop countdown if user closes
        StopCountdown();
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

            // disable all interaction except exit when done
            DisableAllExceptExit();
        }

        // Mark coroutine as finished
        _cycleCoroutine = null;
    }

    // Time-based countdown avoids double-decrements and timing drift
    private IEnumerator CountdownCoroutine()
    {
        float endTime = Time.time + _timeRemaining;
        int lastDisplayed = Mathf.CeilToInt(_timeRemaining);

        // show initial value
        if (timeText != null)
        {
            timeText.text = $"{lastDisplayed}s";
        }

        while (true)
        {
            if (DryerMachineUIPanel != null && !DryerMachineUIPanel.activeSelf)
            {
                // panel closed — stop countdown
                break;
            }

            float remaining = endTime - Time.time;
            int display = Mathf.Max(0, Mathf.CeilToInt(remaining));

            if (display != lastDisplayed && timeText != null)
            {
                timeText.text = $"{display}s";
                lastDisplayed = display;
            }

            if (remaining <= 0f)
            {
                break;
            }

            yield return null;
        }

        if (timeText != null)
        {
            timeText.text = "0";
        }

        _countdownCoroutine = null;
    }

    private void StopCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
    }

    private void OnTempButtonPressed()
    {
        StatusText.text = "Pick Time";
        if (twentyButton != null)
        {
            twentyButton.onClick.AddListener(OnTimeButtonPressed);
        }
        if (fortyButton != null)
        {
            fortyButton.onClick.AddListener(OnTimeButtonPressed);
        }
        if (sixtyButton != null)
        {
            sixtyButton.onClick.AddListener(OnTimeButtonPressed);
        }
    }

    private void OnTimeButtonPressed()
    {
        StatusText.text = "Pick Soil";
        if (SoilButtons != null)
        {
                SoilButtons[0].onClick.AddListener(OnSoilButtonPressed);
                SoilButtons[1].onClick.AddListener(OnSoilButtonPressed);
                SoilButtons[2].onClick.AddListener(OnSoilButtonPressed);
        }
    }

    private void OnSoilButtonPressed()
    {
        StatusText.text = "Ready to Run";
        readyToRun = true;
    }
    private void OnExitButtonPressed()
    {
        DryerMachineUIPanel.SetActive(false);
        StopCountdown();
    }

    // Disable all interactive buttons except the exit button (keeps them visible but non-interactable)
    private void DisableAllExceptExit()
    {
        if (runButton != null) runButton.interactable = false;
        if (twentyButton != null) twentyButton.interactable = false;
        if (fortyButton != null) fortyButton.interactable = false;
        if (sixtyButton != null) sixtyButton.interactable = false;
        if (openButton != null) openButton.interactable = false;
        if (closeButton != null) closeButton.interactable = false;

        if (tempButtons != null)
        {
            foreach (var b in tempButtons)
            {
                if (b != null) b.interactable = false;
            }
        }

        if (SoilButtons != null)
        {
            foreach (var b in SoilButtons)
            {
                if (b != null) b.interactable = false;
            }
        }

        // ensure exit remains usable
        if (exitButton != null) exitButton.interactable = true;
    }

    // Enable all buttons (used when opening UI)
    private void EnableAllButtons()
    {
        if (runButton != null) runButton.interactable = true;
        if (twentyButton != null) twentyButton.interactable = true;
        if (fortyButton != null) fortyButton.interactable = true;
        if (sixtyButton != null) sixtyButton.interactable = true;
        if (openButton != null) openButton.interactable = true;
        if (closeButton != null) closeButton.interactable = true;

        if (tempButtons != null)
        {
            foreach (var b in tempButtons)
            {
                if (b != null) b.interactable = true;
            }
        }

        if (SoilButtons != null)
        {
            foreach (var b in SoilButtons)
            {
                if (b != null) b.interactable = true;
            }
        }

        if (exitButton != null) exitButton.interactable = true;
    }
}