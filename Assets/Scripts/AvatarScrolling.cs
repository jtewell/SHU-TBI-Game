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
    private float scrollAmount = 200f; // The amount to scroll per click
    private VisualElement selectedAvatar; // Store the currently selected avatar




    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        avatarSelectionContainer = root.Q<VisualElement>("avatarSelectionContainer");
        scrollingButtonsContainer = root.Q<VisualElement>("scrollingButtonsContainer");
        avatarSelectionScrollingView = root.Q<ScrollView>("avatarSelectionScrollingView");
        scrollLeft = root.Q<Button>("scrollLeft");
        scrollRight = root.Q<Button>("scrollRight");

        // Register Click Events
        scrollLeft.clicked += ScrollLeftButton;
        scrollRight.clicked += ScrollRightButton;


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
