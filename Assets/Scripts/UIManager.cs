using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public VisualElement overlayUI;

    [SerializeField] private VisualTreeAsset overlay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load the UXML overlay
        InitializeOverlay();
    }

    private void InitializeOverlay()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        if (overlay != null)
        {
            overlayUI = overlay.CloneTree();
            root.Add(overlayUI);
        }
    }
}
