using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MapUIManager : MonoBehaviour
{
    public GameObject MapUI;
    public Button CloseButton, CompassButton;
    public GameObject playerMarker;
    private Vector3 groundDimensions = new Vector3(200, 0, 200);

    public void OnCompassButtonClicked()
    {
        //Player.GetComponent<Player>().ShowCompass();
        Debug.Log("Compass Button Clicked");
    }
    public void OnMapButtonClicked(Transform playerMarker)
    {
        //Player.GetComponent<Player>().ShowMap();
        MapUI.SetActive(true);
        UpdateMapState(playerMarker);
    }

    public void OnCloseButtonClicked()
    {
        MapUI.SetActive(false);
    }

    // get Ground Dimension
    public Vector3 GetGroundDimension()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InitializeGroundDimensions("Ground/Map");
            Vector3 groundDimensions = GameManager.Instance.GroundDimensions;
            //Vector3 groundDimension = GameManager.Instance.InitializeGroundDimensions("Ground/Map");
            //Debug.Log("Ground Dimension from another scene: " + groundDimensions);
            return groundDimensions;
        }
        else
        {
            //Debug.LogError("PlayerManager instance is null!");
            //return 999, 999, 999; // Return a default value if PlayerManager is not assigned
            return Vector3.zero;
        }
    }

    public void GroundSetup (GameObject ground)
    {
        Renderer renderer = ground.GetComponent<Renderer>();

        if (ground != null)
            groundDimensions = renderer.bounds.size;
        else
            groundDimensions = new Vector3(200, 0, 200);

    }

    private void UpdateMapState(Transform playerTransform)
    {

        // Get player's position in world space
        Vector3 playerPos = playerTransform.position;

        // Get map object child
        GameObject MapObject = MapUI.transform.Find("Map").gameObject;

        //Get the map object's rect component
        RectTransform mapDimensions = MapObject.GetComponent<RectTransform>();

        //Get the map's width and height
        float mapWidth = mapDimensions.rect.width;
        float mapHeight = mapDimensions.rect.height;

        //Convert the player's world space position to map coordinates
        float playerX = ((-playerPos.x) / groundDimensions.x) * mapWidth;
        float playerY = ((-playerPos.z) / groundDimensions.z) * mapHeight;

        //Finally! - update the player marker position
        playerMarker.GetComponent<RectTransform>().anchoredPosition = new Vector2(playerX, playerY);

        //Lastly, update the player marker's rotation- first get the player's rotation
        float playerRotationY = playerTransform.rotation.eulerAngles.y;

        //Then apply it to the marker
        playerMarker.transform.rotation = Quaternion.Euler(0, 0, playerRotationY);

        Debug.Log($"Player Position: {playerPos}");
        Debug.Log($"Ground Dimension: {groundDimensions}");
        Debug.Log($"Map Dimensions: {mapWidth}x{mapHeight}");
        Debug.Log($"Player Icon Position: {playerX}, {playerY}");
        Debug.Log($"Player Rotation Y: {playerRotationY}");

    }
}
