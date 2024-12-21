using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;


public class AvatarScrolling : MonoBehaviour
{
    // UI elements references
    public VisualElement avatarContainer, avatarSelectionContainer, scrollingButtonsContainer, selectAvatarConfirmationContainer, 
        avatarGenderSelectionContainer, thankYouMessageContainer, charMessageContainer, previousButtonContainer;
    public ScrollView avatarSelectionScrollingView;
    public Button scrollLeft, scrollRight, yesButton, noButton, maleButton, femaleButton, nonBinaryButton, 
        continueGamePlayButton, previousButton;
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
        charMessageContainer = root.Q<VisualElement>("charMessageContainer");
        previousButtonContainer = root.Q<VisualElement>("previousButtonContainer");

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
        previousButton = root.Q<Button>("previousButton");

        // Register button click events for scrolling and confirmation actions
        scrollLeft.clicked += ScrollLeftButton;
        scrollRight.clicked += ScrollRightButton;
        maleButton.clicked += () => OnGenderSelected("Male");
        femaleButton.clicked += () => OnGenderSelected("Female");
        nonBinaryButton.clicked += () => OnGenderSelected("Non-Binary");
        previousButton.clicked += () => OnPreviousButtonClicked();
        continueGamePlayButton.clicked += () => OnContinueGamePlayButtonClicked();


        // Hide the avatar confirmation container initially
        avatarContainer.style.display = DisplayStyle.None;
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;

        // Register button click events for the Yes/No confirmation
        yesButton.clicked += OnYesButtonClicked;

        // Get all avatar elements in the scroll view and register click events
        var avatarContainerstore = avatarSelectionScrollingView.Query<VisualElement>(className: "avatar").ToList();
        avatarContainerstore.ForEach(avatar => avatar.RegisterCallback<ClickEvent>(ev => OnAvatarClicked(avatar)));
        avatarSelectionScrollingView.RegisterCallback<GeometryChangedEvent>(OnLayoutChanged);

        OnPreviousButtonClicked();

    }
    //create a function to handle previous button click event 
    private void OnPreviousButtonClicked()
    {
        // Hide the current UI elements
        avatarContainer.style.display = DisplayStyle.Flex;
        thankYouMessageContainer.style.display = DisplayStyle.None;
        charMessageContainer.style.backgroundImage = null; // Clear the background image
        previousButtonContainer.style.display = DisplayStyle.Flex;
        
    }
    //on gender selected method
    private void OnGenderSelected(string gender)
    {
        selectedGender = gender;
        Debug.Log("Selected Gender: "+gender);
        // Store the selected gender in the Singleton class
        AvatarSelectionManager.Instance.SetGender(gender);

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
            selectAvatarConfirmationContainer.style.display = DisplayStyle.None;  // Hide confirmation if deselected
            return;
        }
        // Deselect previously selected avatar, if any
        DeselectAvatar();
        // Set the clicked avatar as the new selected avatar
        selectedAvatar = avatar;
        HighlightAvatar(avatar);  // Highlight the newly selected avatar

        // Store the selected avatar in the Singleton class
        AvatarSelectionManager.Instance.SetAvatar(avatar.name);

        // Show and position the confirmation container near the selected avatar
        DisplayConfirmationNearAvatar(avatar);
    }
    //change image of charMessageContainer based on selected avatar
    private void ChangeAvatarBackground(string avatarName)
    {
        // Mapping of avatar containers to character names
        var avatarMapping = new Dictionary<string, string>
    {
        { "avatar1Container", "char1" },
        { "avatar2Container", "char2" },
        { "avatar3Container", "char3" },
        { "avatar4Container", "char4" },
        { "avatar5Container", "char5" }
    };

        // Check if the avatarName exists in the mapping
        if (avatarMapping.TryGetValue(avatarName, out string characterName))
        {
            string imagePath = $"Assets/Art/Textures/Questionnaire_Images/{characterName}_message.png"; // Construct the image path

            // Check if the file exists before trying to load it
            if (System.IO.File.Exists(imagePath))
            {
                byte[] fileData = System.IO.File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData); // Load the image into the texture
                charMessageContainer.style.backgroundImage = new StyleBackground(texture); // Apply the background
            }
            else
            {
                Debug.LogError($"Failed to load background image for avatar: {characterName}. File not found at {imagePath}");
            }
        }
        else
        {
            Debug.LogError($"Failed to load background image for avatar: {avatarName}. Avatar name not recognized.");
        }
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



        // Apply rounded corners to the selected avatar
        selectedAvatar.style.borderTopLeftRadius = new Length(10); // Rounded corners
        selectedAvatar.style.borderTopRightRadius = new Length(10); // Rounded corners
        selectedAvatar.style.borderBottomLeftRadius = new Length(10); // Rounded corners
        selectedAvatar.style.borderBottomRightRadius = new Length(10); // Rounded corners
        //avatar.transform.scale = new Vector3(1.2f, 1.2f, 1.2f);  // Slightly increase size
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


        // Set the position
        selectAvatarConfirmationContainer.style.left = confirmationLeft;
        selectAvatarConfirmationContainer.style.top = confirmationTop;

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
        DeselectAvatar();  // Deselect avatar after confirmation
        selectAvatarConfirmationContainer.style.display = DisplayStyle.None;  // Hide confirmation UI
       // Hide other UI elements if necessary
        HideOtherUIElements(); // Implement this method to hide any additional elements
        // Change the background of the character message container
        ChangeAvatarBackground(selectedAvatar.name);
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

        //Start the game!
        StartGame();
    }
    // Scroll left button logic
    private void ScrollLeftButton()
    {
        // Calculate the new scroll offset and prevent scrolling past the start
        avatarSelectionScrollingView.scrollOffset = new Vector2(
            Mathf.Max(avatarSelectionScrollingView.scrollOffset.x - scrollAmount, 0),  // Ensure it doesn't scroll past 0
            avatarSelectionScrollingView.scrollOffset.y
        );
    }
    // Scroll right button logic
    private void ScrollRightButton()
    {
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

    private void StartGame ()
    {
        //Load the QuestManager with Tutorial Quest (todo)

        //Load the SceneManager with avatar settings (todo)

        //Tell SceneManager to switch to House scene and spawn at the bed
        SceneController.Instance.LoadScene("House", "Bed");
    }
}
