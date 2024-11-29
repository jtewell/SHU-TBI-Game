////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class mapController : MonoBehaviour
////{
//public GameObject Player;
//private VisualElement _playerRepresentation;
//private VisualElement _mapContainer;
//private VisualElement _mapImage;



////    // Start is called before the first frame update
////    void Start()
////    {

////        _root = GetComponent<UIDocument>().rootVisualElement;
////        _mapContainer = _root.Q<VisualElement>("mapContainer");
////        _mapImage = _mapContainer.Q<VisualElement>("detailed_mapVisualElement");
////        _playerRepresentation = _mapImage.Q<VisualElement>("player-icon");


////    }

////    void LateUpdate()
////    {
////        if (!_isMapVisible) return;

////        // Rotate and move the player icon based on the player's movement
////        var multiplyer = IsMapOpen ? fullMultiplyer : miniMultiplyer;
////        _playerRepresentation.style.translate = new Translate(
////            Player.transform.position.x * multiplyer,
////            Player.transform.position.z * -multiplyer,
////            0
////        );
////        _playerRepresentation.style.rotate = new Rotate(new Angle(Player.transform.rotation.eulerAngles.y));

////        // Adjust faded value if the map is open and the player is moving
////        MapFaded = IsMapOpen && PlayerController.Instance.IsMoving;

////        // Move the mini-map
////        if (!IsMapOpen)
////        {
////            // Calculate the width/height bounds for the map image
////            var clampWidth = _mapImage.worldBound.width / 2 - _mapContainer.worldBound.width / 2;
////            var clampHeight = _mapImage.worldBound.height / 2 - _mapContainer.worldBound.height / 2;

////            // Clamp the bounds so that the map doesn't scroll past the playable area (i.e., the map image)
////            var xPos = Mathf.Clamp(
////                Player.transform.position.x * -miniMultiplyer,
////                -clampWidth,
////                clampWidth
////            );
////            var yPos = Mathf.Clamp(
////                Player.transform.position.z * miniMultiplyer,
////                -clampHeight,
////                clampHeight
////            );

////            // Move the map image
////            _mapImage.style.translate = new Translate(xPos, yPos, 0);
////        }
////        else
////        {
////            // Reset map translation when in full mode
////            _mapImage.style.translate = new Translate(0, 0, 0);
////        }
////    }

////    /// <summary>
////    /// Check if the map is in "full" mode
////    /// </summary>
////    private bool IsMapOpen => _root.ClassListContains("root-container-full");

////    /// <summary>
////    /// Toggle between full and mini mode
////    /// </summary>
////    /// <param name="on">Should the map be in full mode?</param>
////    private void ToggleMap(bool on)
////    {
////        _root.EnableInClassList("root-container-mini", !on);
////        _root.EnableInClassList("root-container-full", on);
////    }

////    // Update is called once per frame
////    void Update()
////    {

////    }
////}
