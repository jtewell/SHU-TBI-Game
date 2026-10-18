using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class WashingMachineUI : MonoBehaviour
{
    public Button runButton;
    public Button exitButton;
    public Button openButton;
    public Button closeButton;
    public Button[] tempButtons; // 0 = cold, 1 = warm, 2 = hot
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI StatusText;

    [SerializeField]
    private InteractableWasherMachine washerMachine;

    public GameObject WashingMachineUIPanel;

    [Header("Washer Cycling")]
    public Sprite[] Sprites;
    public Sprite MachineOpen;
    public Sprite MachineOpenDone;

    public float FrameInterval = 0.2f;

    [Header("Cycle Duration")]
    public float CycleDuration = 10f;

    [Header("Completion")]
    public Sprite FinishedSprite;

    private WashingMachineUI washingMachineUI;

    [Header("Targets")]
    public Image TargetImage;
    public SpriteRenderer TargetRenderer;

    private int _currentIndex;
    private Coroutine _cycleCoroutine;
    private bool _stopRequested;

    // Countdown state
    private Coroutine _countdownCoroutine;
    private float _timeRemaining;

    private bool[] tempSelected = new bool[3];
    private bool closeButtonPressed = false;

    private bool machineOpen = false;

    private void OnEnable()
    {
        if (runButton != null)
        {
            runButton.onClick.AddListener(OnRunButtonPressed);
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
            if (closeButtonPressed = false)
            {
                tempButtons[0].onClick.AddListener(() => { tempSelected[0] = true; });
                tempButtons[1].onClick.AddListener(() => { tempSelected[1] = true; });
                tempButtons[2].onClick.AddListener(() => { tempSelected[2] = true; });
            }
            if (closeButtonPressed = true)
            {
                tempButtons[0].onClick.AddListener(() => { StatusText.text = "Hit Run"; });
                tempButtons[1].onClick.AddListener(() => { StatusText.text = "Hit Run"; });
                tempButtons[2].onClick.AddListener(() => { StatusText.text = "Hit Run"; });
            }
        }

    }

    private void OnDisable()
    {
        if (runButton != null)
        {
            runButton.onClick.RemoveListener(OnRunButtonPressed);
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

        if (WashingMachineUIPanel != null)
        {
            WashingMachineUIPanel.SetActive(false);

            // Auto-assign washingMachineUI if not set in inspector
            if (washingMachineUI == null)
            {
                washingMachineUI = WashingMachineUIPanel.GetComponentInChildren<WashingMachineUI>();
            }
        }

        // Ensure sane defaults
        FrameInterval = Mathf.Max(0.02f, FrameInterval);
        CycleDuration = Mathf.Max(0.02f, CycleDuration);

        // Initialize time text if present
        if (timeText != null)
        {
            timeText.text = "0s";
        }

        if (StatusText != null)
        {
            StatusText.text = "Idle";
        }
    }

    public void OpenWashingMachineUI()
    {
        if (WashingMachineUIPanel != null)
        {
            WashingMachineUIPanel.SetActive(true);
            // Ensure buttons are enabled when opening UI
            EnableButtonsExceptExit();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void InitializeRunButton()
    {
        if (runButton != null)
        {
            runButton.onClick.AddListener(OnRunButtonPressed);
        }
    }

    public void InitializeCloseButton()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
        }
    }

    private void OnRunButtonPressed()
    {
        // Stop any existing countdown
        StopCountdown();

        // Stop any existing cycle
        if (_cycleCoroutine != null)
        {
            _stopRequested = true;
            StopCoroutine(_cycleCoroutine);
            _cycleCoroutine = null;
        }

        // Set countdown to 10 seconds and show it
        _timeRemaining = 10f;
        if (timeText != null)
        {
            timeText.text = $"{Mathf.CeilToInt(_timeRemaining)}s";
        }

        // Use the countdown value as the cycle duration so both run for the same time
        CycleDuration = _timeRemaining;

        // Start countdown
        _countdownCoroutine = StartCoroutine(CountdownCoroutine());

        // Validate sprites
        if (Sprites == null || Sprites.Length == 0)
        {
            Debug.LogWarning("WashingMachineUI: No sprites assigned to cycle.");
            return;
        }

        // Start sprite cycle
        _stopRequested = false;
        _currentIndex = 0;
        _cycleCoroutine = StartCoroutine(CycleSprites());

        if (StatusText != null)
        {
            StatusText.text = "Running";
        }
    }

    private void OnOpenButtonPressed()
    {
        
        if (!machineOpen)
        {
            TargetImage.sprite = MachineOpen;
            machineOpen = true;
        }
        else
        {
            TargetImage.sprite = MachineOpenDone;
            InitializeCloseButton();
        }
    }
    private void OnCloseButtonPressed()
    {
        if (TargetImage != null && Sprites != null && Sprites.Length > 0)
        {
            TargetImage.sprite = Sprites[0];
        }
        InitializeRunButton();
        StopCountdown();
        if (StatusText != null && (tempSelected[0] == false || tempSelected[1] == false || tempSelected[2] == false))
        {
            StatusText.text = "Pick Temp";
        }
        if (StatusText != null && (tempSelected[0] == true || tempSelected[1] == true || tempSelected[2] == true))
        {
            closeButtonPressed = true;
        }

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
        while (!_stopRequested && (WashingMachineUIPanel == null || WashingMachineUIPanel.activeSelf) && Time.time < endTime)
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


            // Disable all buttons except exit when the machine is done
            DisableButtonsExceptExit();
        }
        else
        {
            if (StatusText != null)
            {
                StatusText.text = "Stopped";
            }
        }

        // Mark coroutine as finished
        _cycleCoroutine = null;
    }

    // Countdown coroutine for the timeText
    private IEnumerator CountdownCoroutine()
    {
        while (_timeRemaining > 0f && (WashingMachineUIPanel == null || WashingMachineUIPanel.activeSelf))
        {
            yield return new WaitForSeconds(1f);
            _timeRemaining -= 1f;
            if (timeText != null)
            {
                timeText.text = $"{Mathf.CeilToInt(Mathf.Max(0f, _timeRemaining))}s";
            }
        }

        // Ensure final text shows 0s when finished
        if (timeText != null)
        {
            timeText.text = "0s";
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

    private void OnExitButtonPressed()
    {
        if (WashingMachineUIPanel != null)
        {
            WashingMachineUIPanel.SetActive(false);
        }
        StopCountdown();
    }

    // Disable all interactive buttons except the exit button
    private void DisableButtonsExceptExit()
    {
        if (runButton != null) runButton.gameObject.SetActive(false);
        if (openButton != null) openButton.gameObject.SetActive(false);
        if (closeButton != null) closeButton.gameObject.SetActive(false);
        if (tempButtons != null)
        {
            foreach (var b in tempButtons)
            {
                if (b != null) b.gameObject.SetActive(false);
            }
        }
        // leave exitButton alone
    }

    // Re-enable buttons (except exit) when opening/closing UI
    private void EnableButtonsExceptExit()
    {
        if (runButton != null) runButton.gameObject.SetActive(true);
        if (openButton != null) openButton.gameObject.SetActive(true);
        if (closeButton != null) closeButton.gameObject.SetActive(true);
        if (tempButtons != null)
        {
            foreach (var b in tempButtons)
            {
                if (b != null) b.gameObject.SetActive(true);
            }
        }
    }

}



