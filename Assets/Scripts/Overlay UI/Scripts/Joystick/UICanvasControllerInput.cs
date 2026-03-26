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
        Color buttonUp = new Color(1.0f,1.0f,1.0f,1.0f);
        Color buttonDown = new Color(.75f,.75f,.75f,1.0f);
        
        public Image sprintButtonImage;
        public Sprite walkingImage;
        public Sprite sprintingImage;

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

    

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput()
        {
            isSprinting = !isSprinting;
            starterAssetsInputs.SprintInput(isSprinting);
            sprintButton.image.color = isSprinting ? buttonDown : buttonUp;
            sprintButtonImage.sprite = isSprinting ? walkingImage : sprintingImage;
        }

    }

}
