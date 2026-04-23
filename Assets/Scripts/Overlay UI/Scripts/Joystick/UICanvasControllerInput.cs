using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        public bool isSprinting = false;
        public Button sprintButton;
        Color buttonUp = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color buttonDown = new Color(.75f, .75f, .75f, 1.0f);

        public Image sprintButtonImage;
        public Sprite walkingImage;
        public Sprite sprintingImage;

        [Header("Output")]
        private StarterAssetsInputs starterAssetsInputs;

        private void Awake()
        {
            if (starterAssetsInputs == null)
            {
                starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
            }

            if (starterAssetsInputs == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
                }
            }

            if (starterAssetsInputs == null)
            {
                Debug.LogWarning("UICanvasControllerInput could not find a StarterAssetsInputs component in the scene.");
            }
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.MoveInput(virtualMoveDirection);
            }
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.LookInput(virtualLookDirection);
            }
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.JumpInput(virtualJumpState);
            }
        }

        public void VirtualSprintInput()
        {
            isSprinting = !isSprinting;

            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.SprintInput(isSprinting);
            }

            sprintButton.image.color = isSprinting ? buttonDown : buttonUp;
            sprintButtonImage.sprite = isSprinting ? walkingImage : sprintingImage;
        }
    }
}