using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanMap : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public RectTransform mapImageRect;
    public GameObject playerMarker;
    public float panSpeed = 1f;
    public Button compassButton;

    //Internal variables
    private float borderX = 0;
    private float borderY = 0;
    private Vector2 pointerLocalPos;

    private void Start()
    {
        //Setup the listener for the compass
        compassButton.onClick.AddListener(CenterOnPlayer);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //Get the initial scale of the map in local coords
        float originalMapScale = mapImageRect.localScale.x;

        //Get the width and height of the map in local coords
        float viewWidth = mapImageRect.rect.width;
        float viewHeight = mapImageRect.rect.height;

        // Figure out how far from center the edges are, in local coords
        borderX = (mapImageRect.rect.width * originalMapScale - viewWidth) * 0.5f;
        borderY = (mapImageRect.rect.height * originalMapScale - viewHeight) * 0.5f;

        // Convert mouse/finger position from screen to local (parent) coords
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImageRect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out pointerLocalPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Current mouse/finger position in same local space
        Vector2 currentLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImageRect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out currentLocalPos);

        // Delta since last frame
        Vector2 delta = currentLocalPos - pointerLocalPos;
        pointerLocalPos = currentLocalPos;

        // Move the map
        mapImageRect.anchoredPosition += delta * panSpeed;

        // Clamp to borders
        Vector2 clamped = mapImageRect.anchoredPosition;
        clamped.x = Mathf.Clamp(clamped.x, -borderX, borderX);
        clamped.y = Mathf.Clamp(clamped.y, -borderY, borderY);
        mapImageRect.anchoredPosition = clamped;
    }

    private void CenterOnPlayer()
    {
        // Get the marker's local position inside the map
        RectTransform playerMarkerRT = playerMarker.GetComponent<RectTransform>();
        Vector2 playerMarkerLocalPosition = playerMarkerRT.anchoredPosition;

        // To put the marker at the exact center of the viewport, the map's anchoredPosition needs to be the negative of that
        Vector2 desiredPosition = -playerMarkerLocalPosition;

        // Clamp to the borders so the player never pans off-map
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -borderX, borderX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, -borderY, borderY);

        // 4) Apply it!
        mapImageRect.anchoredPosition = desiredPosition;
    }
}
