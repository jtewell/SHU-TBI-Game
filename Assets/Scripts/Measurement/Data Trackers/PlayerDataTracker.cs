using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerDataTracker : MonoBehaviour
{
    [Tooltip("How slow the player must be to have stopped")]
    [SerializeField] private float stopThreshold;

    private bool wasMoving = false;
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // initialize the stopping state when the game begins
        wasMoving = characterController.velocity.magnitude > stopThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        // Detect if the player is moving
        bool isMoving = characterController.velocity.magnitude > stopThreshold;

        //If the player is not moving but was moving before, then record the stop
        if (!isMoving && wasMoving)
        {
            MeasurementDataManager.Instance.numberTimesStopped += 1;
        }

        //Update for the next frame
        wasMoving = isMoving;

        //Calculate the amount of distance traveled in the last frame update
        float deltaDistance = characterController.velocity.magnitude * Time.deltaTime;
        MeasurementDataManager.Instance.distanceTraveled += deltaDistance;
    }
}
