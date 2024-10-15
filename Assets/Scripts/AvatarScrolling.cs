using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;


public class AvatarScrolling : MonoBehaviour
{
    // UI elements references
    public VisualElement avatarContainer, avatarSelectionContainer, scrollingButtonsContainer, selectAvatarConfirmationContainer, avatarGenderSelectionContainer, thankYouMessageContainer;
    public ScrollView avatarSelectionScrollingView;
    public Button scrollLeft, scrollRight, yesButton, noButton, maleButton, femaleButton, nonBinaryButton, continueGamePlayButton;
    public Label titleAvatarSelection, thankYouLabel;
    private string selectedGender;

    // Scroll amount per click
    private float scrollAmount = 200f;

    // Reference to the currently selected avatar
    private VisualElement selectedAvatar;

    void Start()
    {
        // Get the root UI document
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Initialize references to the visual elements from the UI
        avatarContainer = root.Q<VisualElement>("avatarContainer");
        avatarSelectionContainer = root.Q<VisualElement>("avatarSelectionContainer");
        scrollingButtonsContainer = root.Q<VisualElement>("scrollingButtonsContainer");
        avatarSelectionScrollingView = root.Q<ScrollView>("avatarSelectionScrollingView");
        selectAvatarConfirmationContainer = root.Q<VisualElement>("selectAvatarConfirmationContainer");
        avatarGenderSelectionContainer = root.Q<VisualElement>("avatarGenderSelectionContainer");
        thankYouMessageContainer = root.Q<VisualElement>("thankYouMessageContainer");

        // Initialize buttons and labels
        yesButton = root.Q<Button>("yesButton");
        noButton = root.Q<Button>("noButton");
        scrollLeft = root.Q<Button>("scrollLeft");
        scrollRight = root.Q<Button>("scrollRight");
        maleButton = root.Q<Button>("maleButton");
        femaleButton = root.Q<Button>("femaleButton");
        nonBinaryButton = root.Q<Button>("nonBinaryButton");
        titleAvatarSelection = root.Q<Label>("titleAvatarSelection");
        thankYouLabel = root.Q<Label>("thankYouLabel");
        continueGamePlayButton = root.Q<Button>("continueGamePlayButton");

        // Register button click events for scrolling and confirmation actions
        scrollLeft.clicked += ScrollLeftButton;
        scrollRight.clicked += ScrollRightButton;
        maleButton.clicked += () => OnGenderSelected("Male");
        femaleButton.clicked += () => OnGenderSelected("Female");
        nonBinaryButton.clicked += () => OnGenderSelected("Non-Binary");


        // Hide the avatar confirmation container initially
        avatarContainer.style.display = DisplayStyle.None;
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;

        // Register button click events for the Yes/No confirmation
        yesButton.clicked += OnYesButtonClicked;

        // Get all avatar elements in the scroll view and register click events
        var avatarContainerstore = avatarSelectionScrollingView.Query<VisualElement>(className: "avatar").ToList();
        avatarContainerstore.ForEach(avatar => avatar.RegisterCallback<ClickEvent>(ev => OnAvatarClicked(avatar)));
        avatarSelectionScrollingView.RegisterCallback<GeometryChangedEvent>(OnLayoutChanged);

    }
    //on gender selected method
    private void OnGenderSelected(string gender)
    {
        selectedGender = gender;
        Debug.Log("Selected Gender: "+gender);
        avatarGenderSelectionContainer.style.display = DisplayStyle.None;
        avatarContainer.style.display = DisplayStyle.Flex;

        // Select the first avatar by default when the avatar container is shown
        //SelectFirstAvatarByDefault();
    }

    // Handle avatar click event
    private void OnAvatarClicked(VisualElement avatar)
    {
        Debug.Log("Avatar clicked: " + avatar.name);

        // If the clicked avatar is already selected, deselect it
        if (selectedAvatar == avatar)
        {
            DeselectAvatar();  // Deselect current avatar
            selectAvatarConfirmationContainer.style.display = DisplayStyle.None;  // Hide confirmation if deselected
            return;
        }

        // Deselect previously selected avatar, if any
        DeselectAvatar();

        // Set the clicked avatar as the new selected avatar
        selectedAvatar = avatar;
        HighlightAvatar(avatar);  // Highlight the newly selected avatar

        // Show and position the confirmation container near the selected avatar
        DisplayConfirmationNearAvatar(avatar);
        // Show the confirmation container for avatar selection
        //selectAvatarConfirmationContainer.style.display = DisplayStyle.Flex;
    }
    // Handle layout change event
    private void OnLayoutChanged(GeometryChangedEvent evt)
    {
        if (selectedAvatar != null)
        {
            DisplayConfirmationNearAvatar(selectedAvatar);
        }
    }

    // Deselect the currently selected avatar
    private void DeselectAvatar()
    {
        if (selectedAvatar != null)
        {
            // Remove selection highlight and reset avatar appearance
            selectedAvatar.RemoveFromClassList("selected-avatar");
            selectedAvatar.style.backgroundColor = new Color(1, 1, 1, 0); // Transparent background
            selectedAvatar.transform.scale = Vector3.one;  // Reset scale to default

            // Hide the confirmation container when deselecting the avatar
            selectAvatarConfirmationContainer.style.display = DisplayStyle.None;
        }
    }

    // Highlight the newly selected avatar
    private void HighlightAvatar(VisualElement avatar)
    {
        // Add a highlight class, change background color, apply corner radius and scale
        avatar.AddToClassList("selected-avatar");
        avatar.style.backgroundColor = new Color(0, 0, 139 / 255f, 0.5f); // Semi-transparent blue background
        selectedAvatar.style.borderTopLeftRadius = new Length(10); // Rounded corners
        selectedAvatar.style.borderTopRightRadius = new Length(10); // Rounded corners
        selectedAvatar.style.borderBottomLeftRadius = new Length(10); // Rounded corners
        selectedAvatar.style.borderBottomRightRadius = new Length(10); // Rounded corners
        avatar.transform.scale = new Vector3(1.2f, 1.2f, 1.2f);  // Slightly increase size
    }
    // Method to position the confirmation container near the selected avatar
    private void DisplayConfirmationNearAvatar(VisualElement avatar)
    {
        // Set the confirmation container to visible
        selectAvatarConfirmationContainer.style.display = DisplayStyle.Flex;

        // Set position to absolute
        selectAvatarConfirmationContainer.style.position = Position.Absolute;

        // Bring the confirmation container to the front
        selectAvatarConfirmationContainer.BringToFront();

        // Wait for the next frame to ensure the layout is calculated
        // This is to ensure resolvedStyle dimensions are accurate after setting display
        StartCoroutine(UpdateConfirmationPosition(avatar));
    }
    private IEnumerator UpdateConfirmationPosition(VisualElement avatar)
    {
        // Wait for the next frame to get accurate sizes
        yield return null;

        // Get the position of the avatar in world space
        var avatarWorldBound = avatar.worldBound;

        // Calculate the position above the avatar
        float confirmationLeft = avatarWorldBound.x + (avatarWorldBound.width - selectAvatarConfirmationContainer.resolvedStyle.width) / 2; // Center horizontally
        float confirmationTop = avatarWorldBound.y - selectAvatarConfirmationContainer.resolvedStyle.height - 15; // Position above the avatar

        // Debug logs for position calculations
        Debug.Log("Avatar Position: " + avatarWorldBound);
        Debug.Log("Calculated Confirmation Position: (left: " + confirmationLeft + "px, top: " + confirmationTop + "px)");

        // Set the position
        selectAvatarConfirmationContainer.style.left = confirmationLeft;
        selectAvatarConfirmationContainer.style.top = confirmationTop;

        // Log the size of the confirmation container for debugging
        Debug.Log("Confirmation Container Size: (width: " + selectAvatarConfirmationContainer.resolvedStyle.width + ", height: " + selectAvatarConfirmationContainer.resolvedStyle.height + ")");

        // Set the background color to dark blue
        selectAvatarConfirmationContainer.style.backgroundColor = new Color(0, 0, 139 / 255f, 0.8f); // Dark blue color

        // Apply rounded corners to the container
        selectAvatarConfirmationContainer.style.borderTopLeftRadius = new Length(10);
        selectAvatarConfirmationContainer.style.borderTopRightRadius = new Length(10);
        selectAvatarConfirmationContainer.style.borderBottomLeftRadius = new Length(10);
        selectAvatarConfirmationContainer.style.borderBottomRightRadius = new Length(10);

        // Ensure the layout updates
        selectAvatarConfirmationContainer.MarkDirtyRepaint();
    }

    // Handle "Yes" button click - confirm avatar selection
    private void OnYesButtonClicked()
    {
        Debug.Log("Avatar selection confirmed: " + selectedAvatar.name);
        DeselectAvatar();  // Deselect avatar after confirmation
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;  // Hide confirmation UI
       // Hide other UI elements if necessary
        HideOtherUIElements(); // Implement this method to hide any additional elements
        // Show the thank you message
        ShowThankYouMessage();
    }
    //Hide other UI elements when click on "Yes" button
    private void HideOtherUIElements()
    {
         // Hide the scrolling buttons container
        avatarContainer.style.display = DisplayStyle.None;
    }
    // Show the thank you message
    private void ShowThankYouMessage()
    {
        // Show the thank you message container
        thankYouMessageContainer.style.display = DisplayStyle.Flex;
    }

    private void OnContinueGamePlayButtonClicked()
    {
        // Hide the confirmation container
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;

        // Hide other UI elements if necessary
        HideOtherUIElements();

        // Show the thank you message
        ShowThankYouMessage();
    }

    // Scroll left button logic
    private void ScrollLeftButton()
    {
        Debug.Log("Scrolling left");

        // Calculate the new scroll offset and prevent scrolling past the start
        avatarSelectionScrollingView.scrollOffset = new Vector2(
            Mathf.Max(avatarSelectionScrollingView.scrollOffset.x - scrollAmount, 0),  // Ensure it doesn't scroll past 0
            avatarSelectionScrollingView.scrollOffset.y
        );
    }

    // Scroll right button logic
    private void ScrollRightButton()
    {
        Debug.Log("Scrolling right");

        // Calculate the maximum possible scroll offset (content width minus visible area width)
        float maxScrollOffsetX = avatarSelectionScrollingView.contentContainer.worldBound.width - avatarSelectionScrollingView.worldBound.width;

        // Scroll and ensure it doesn't exceed the maximum scroll offset
        avatarSelectionScrollingView.scrollOffset = new Vector2(
            Mathf.Min(avatarSelectionScrollingView.scrollOffset.x + scrollAmount, maxScrollOffsetX),  // Scroll within the limits
            avatarSelectionScrollingView.scrollOffset.y
        );
    }

    // Empty update method (in case needed for future use)
    void Update() { }
}
