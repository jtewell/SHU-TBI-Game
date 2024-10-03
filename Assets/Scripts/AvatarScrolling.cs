using UnityEngine;
using UnityEngine.UIElements;

public class AvatarScrolling : MonoBehaviour
{
    // UI elements references
    public VisualElement avatarContainer, avatarSelectionContainer, scrollingButtonsContainer, selectAvatarConfirmationContainer, avatarGenderSelectionContainer;
    public ScrollView avatarSelectionScrollingView;
    public Button scrollLeft, scrollRight, yesButton, noButton, maleButton, femaleButton, nonBinaryButton;
    public Label titleAvatarSelection;
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

        // Initialize buttons and labels
        yesButton = root.Q<Button>("yesButton");
        noButton = root.Q<Button>("noButton");
        scrollLeft = root.Q<Button>("scrollLeft");
        scrollRight = root.Q<Button>("scrollRight");
        maleButton = root.Q<Button>("maleButton");
        femaleButton = root.Q<Button>("femaleButton");
        nonBinaryButton = root.Q<Button>("nonBinaryButton");
        titleAvatarSelection = root.Q<Label>("titleAvatarSelection");

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
        noButton.clicked += OnNoButtonClicked;

        // Get all avatar elements in the scroll view and register click events
        var avatarContainerstore = avatarSelectionScrollingView.Query<VisualElement>(className: "avatar").ToList();
        avatarContainerstore.ForEach(avatar => avatar.RegisterCallback<ClickEvent>(ev => OnAvatarClicked(avatar)));
    }
    //on gender selected method
    private void OnGenderSelected(string gender)
    {
        selectedGender = gender;
        Debug.Log("Selected Gender: "+gender);
        avatarGenderSelectionContainer.style.display = DisplayStyle.None;
        avatarContainer.style.display = DisplayStyle.Flex;
    }
    // Handle avatar click event
    private void OnAvatarClicked(VisualElement avatar)
    {
        Debug.Log("Avatar clicked: " + avatar.name);

        // If the clicked avatar is already selected, deselect it
        if (selectedAvatar == avatar)
        {
            DeselectAvatar();  // Deselect current avatar
            return;
        }

        // Deselect previously selected avatar, if any
        DeselectAvatar();

        // Set the clicked avatar as the new selected avatar
        selectedAvatar = avatar;
        HighlightAvatar(avatar);  // Highlight the newly selected avatar

        // Show the confirmation container for avatar selection
        selectAvatarConfirmationContainer.style.display = DisplayStyle.Flex;
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

    // Handle "Yes" button click - confirm avatar selection
    private void OnYesButtonClicked()
    {
        Debug.Log("Avatar selection confirmed: " + selectedAvatar.name);
        DeselectAvatar();  // Deselect avatar after confirmation
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;  // Hide confirmation UI
    }

    // Handle "No" button click - cancel avatar selection
    private void OnNoButtonClicked()
    {
        Debug.Log("Avatar selection canceled.");
        DeselectAvatar();  // Deselect the current avatar
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;  // Hide confirmation UI
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
