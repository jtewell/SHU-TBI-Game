using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AvatarScrolling : MonoBehaviour

{
    public VisualElement avatarSelectionContainer;
    public VisualElement scrollingButtonsContainer;
    public ScrollView avatarSelectionScrollingView;
    public Button scrollLeft;
    public Button scrollRight;
    public VisualElement selectAvatarConfirmationContainer; // Reference to the confirmation container
    public Button yesButton;
    public Button noButton;
    public Label titleAvatarSelection;
    private float scrollAmount = 200f; // The amount to scroll per click
    private VisualElement selectedAvatar; // Store the currently selected avatar




    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Get the visual elements from the UIDocument
        avatarSelectionContainer = root.Q<VisualElement>("avatarSelectionContainer");
        scrollingButtonsContainer = root.Q<VisualElement>("scrollingButtonsContainer");
        avatarSelectionScrollingView = root.Q<ScrollView>("avatarSelectionScrollingView");
        selectAvatarConfirmationContainer = root.Q<VisualElement>("selectAvatarConfirmationContainer");

        // Get the buttons and labels
        yesButton = root.Q<Button>("yesButton");
        noButton = root.Q<Button>("noButton");
        scrollLeft = root.Q<Button>("scrollLeft");
        scrollRight = root.Q<Button>("scrollRight");
        titleAvatarSelection = root.Q<Label>("titleAvatarSelection");

        // Register Click Events
        scrollLeft.clicked += ScrollLeftButton;
        scrollRight.clicked += ScrollRightButton;

        // Hide confirmation container initially
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;
        // Register button click events
        yesButton.clicked += OnYesButtonClicked;
        noButton.clicked += OnNoButtonClicked;


        // Get all avatar containers and register click events
        var avatarContainers = avatarSelectionScrollingView.Query<VisualElement>(className: "avatar").ToList();
        foreach (var avatar in avatarContainers)
        {
            avatar.RegisterCallback<ClickEvent>(ev => OnAvatarClicked(avatar));
        }
    }
    // Handle avatar click event
    private void OnAvatarClicked(VisualElement avatar)
    {
        Debug.Log("Avatar clicked: " + avatar.name); // Log the avatar clicked

        // If the clicked avatar is already selected, deselect it
        if (selectedAvatar == avatar)
        {
            selectedAvatar.RemoveFromClassList("selected-avatar");
            selectedAvatar.style.backgroundColor = new Color(1, 1, 1, 0); // Reset background to transparent
            selectedAvatar.transform.scale = new Vector3(1f, 1f, 1f); // Reset scale if necessary
            selectedAvatar = null; // Clear selection
            return;
        }
        // Remove selection highlight from the previously selected avatar
        if (selectedAvatar != null)
        {
            selectedAvatar.RemoveFromClassList("selected-avatar");
            selectedAvatar.style.backgroundColor = new Color(1, 1, 1, 0); // Reset background to transparent
            // Optionally reset the scale if necessary
            selectedAvatar.transform.scale = new Vector3(1f, 1f, 1f);
            //Vector3.one;
        }

        // Highlight the clicked avatar
        selectedAvatar = avatar;
        selectedAvatar.AddToClassList("selected-avatar");
        selectedAvatar.style.backgroundColor = new Color(0, 0, 139 / 255f, 0.5f); // Set background to yellow
        selectedAvatar.style.borderTopLeftRadius = new Length(15); // Rounded corners
        selectedAvatar.style.borderTopRightRadius = new Length(15); // Rounded corners
        selectedAvatar.style.borderBottomLeftRadius = new Length(15); // Rounded corners
        selectedAvatar.style.borderBottomRightRadius = new Length(15); // Rounded corners
        selectedAvatar.transform.scale = new Vector3(1.2f, 1.2f, 1.2f);

        // Show the confirmation container
        selectAvatarConfirmationContainer.style.display = DisplayStyle.Flex;
        //titleAvatarSelection.style.display = DisplayStyle.None;
    }
    // Handle "Yes" button click
    private void OnYesButtonClicked()
    {
        Debug.Log("Avatar selection confirmed: " + selectedAvatar.name);
        // Add your logic for confirming the avatar selection here

        // Hide the confirmation container
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;
    }

    // Handle "No" button click
    private void OnNoButtonClicked()
    {
        Debug.Log("Avatar selection canceled.");
        // Reset the selected avatar
        if (selectedAvatar != null)
        {
            selectedAvatar.RemoveFromClassList("selected-avatar");
            selectedAvatar = null;
        }

        // Hide the confirmation container
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;
    }


    //add logic for scrolling left
    void ScrollLeftButton()
    {
        Debug.Log("i'm inside scroll left");
        avatarSelectionScrollingView.scrollOffset = new Vector2(avatarSelectionScrollingView.scrollOffset.x - scrollAmount, avatarSelectionScrollingView.scrollOffset.y);
    }

    //add logic for scrolling right
    void ScrollRightButton()
    {
        Debug.Log("i'm inside scroll right");
        avatarSelectionScrollingView.scrollOffset = new Vector2(avatarSelectionScrollingView.scrollOffset.x + scrollAmount, avatarSelectionScrollingView.scrollOffset.y);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
