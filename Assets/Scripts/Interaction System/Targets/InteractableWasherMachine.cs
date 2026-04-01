
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static InteractableElevator;

[System.Serializable] public class UseWashingMachine : UnityEvent<string> { }

public class InteractableWasherMachine : MonoBehaviour
{
    public static UseWashingMachine useWashingMachine = new UseWashingMachine();

    public GameObject WashingMachineUIPanel;

    [Header("Sprite Cycling")]
    public Sprite[] Sprites;
    public Sprite MachineOpen;
    public Sprite MachineOpenDone;

    [Tooltip("Seconds between frames. Minimum 0.02f. Used only when Loop is true.")]
    public float FrameInterval = 0.2f;

    [Header("Cycle Duration (non-loop)")]
    [Tooltip("Total seconds the washing cycle should take when Loop is false. Per-frame interval is calculated as CycleDuration / frameCount and clamped to a minimum of 0.02s.")]
    public float CycleDuration = 0f;

    [Header("Completion")]
    [Tooltip("Sprite to display when the washing cycle finishes (used when Loop is false). If null, the last frame of `Sprites` will be shown.")]
    public Sprite FinishedSprite;

    [Header("Targets (assign one)")]
    public Image TargetImage;
    public SpriteRenderer TargetRenderer;

    private int _currentIndex;
    private Coroutine _cycleCoroutine;
    private bool _stopRequested;

    private WashingMachineUI washingMachineUI;

    private bool tempButtonPressed = false;
    public void Start()
    {
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
    }

    public void Interact()
    {
        OpenWashingMachineUI();
    }

    public void OpenWashingMachineUI()
    {
        if (WashingMachineUIPanel != null)
        {
            WashingMachineUIPanel.SetActive(true);
            TargetImage.sprite = FinishedSprite;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnRunButtonHit()
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

            if (Sprites == null || Sprites.Length == 0)
            {
                Debug.LogWarning("InteractableWasherMachine: No sprites assigned to cycle.");
                return;
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

    public void OnTwentyButtonHit()
    {
        CycleDuration = 5f;
    }

    public void OnFortyButtonHit()
    {
        CycleDuration = 10f;
    }

    public void OnSixtyButtonHit()
    {
        CycleDuration = 15f;
    }
    public void OnOpenButtonHit()
    {
        TargetImage.sprite = MachineOpen;
        washingMachineUI.InitializeCloseButton();
    }
    public void OnCloseButtonHit()
    {
        TargetImage.sprite = Sprites[0];
        washingMachineUI.InitializeRunButton();
    }

    public void OnTempButtonHit()
    {
        tempButtonPressed = true;
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
        }

        // Mark coroutine as finished
        _cycleCoroutine = null;
    }

    public void ExitWashingMachineUI()
    {
        // Stop cycling when the UI is closed
        _stopRequested = true;
        if (_cycleCoroutine != null)
        {
            StopCoroutine(_cycleCoroutine);
            _cycleCoroutine = null;
        }

        // Optionally reset displayed sprite to first frame
        _currentIndex = 0;
        if (Sprites != null && Sprites.Length > 0)
        {
            Sprite first = Sprites[0];
            if (TargetImage != null)
            {
                TargetImage.sprite = first;
            }
            else if (TargetRenderer != null)
            {
                TargetRenderer.sprite = first;
            }
        }

        WashingMachineUIPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        _stopRequested = true;
        if (_cycleCoroutine != null)
        {
            StopCoroutine(_cycleCoroutine);
            _cycleCoroutine = null;
        }
    }
}
