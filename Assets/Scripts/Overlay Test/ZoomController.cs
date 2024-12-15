using UnityEngine;
using UnityEngine.UI;


public class ZoomController : MonoBehaviour
{
    public RectTransform imageRect; // The image to zoom
    public RectTransform playerRect;     // The target visual element (player)

    public Button zoomInButton;     // The button to zoom in
    public Button zoomOutButton;    // The button to zoom out
    public float zoomSpeed = 0.1f;  // Speed of zooming
    public float minScale = 0.6f;   // Minimum zoom scale 
    public float maxScale = 2.0f;   // Maximum zoom scale


    private void Start()
    {
        // Add listeners for buttons
        zoomInButton.onClick.AddListener(ZoomIn);
        zoomOutButton.onClick.AddListener(ZoomOut);
    }

    public void ZoomIn()
    {
        // Increase the scale, clamped to the maximum value
        Vector3 newScale = imageRect.localScale + Vector3.one * zoomSpeed;
        imageRect.localScale = Vector3.Min(newScale, Vector3.one * maxScale);
    }

    public void ZoomOut()
    {
        // Decrease the scale, clamped to the minimum value
        Vector3 newScale = imageRect.localScale - Vector3.one * zoomSpeed;
        imageRect.localScale = Vector3.Max(newScale, Vector3.one * minScale);
    }

    private void AdjustZoom(Vector3 zoomDelta)
    {
        // Calculate new scale
        Vector3 newScale = imageRect.localScale + zoomDelta;
        newScale = Vector3.Max(Vector3.one * minScale, Vector3.Min(newScale, Vector3.one * maxScale));
        imageRect.localScale = newScale;

        // Adjust pivot to keep player in focus
        Vector2 pivot = new Vector2(
            (playerRect.anchoredPosition.x + imageRect.sizeDelta.x / 2) / imageRect.sizeDelta.x,
            (playerRect.anchoredPosition.y + imageRect.sizeDelta.y / 2) / imageRect.sizeDelta.y
        );
        imageRect.pivot = pivot;

        // Optionally reposition the container
        imageRect.anchoredPosition = Vector2.zero; // Adjust as needed
    }
}
