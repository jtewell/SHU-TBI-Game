using UnityEngine;

public class AvatarSelectionManager : MonoBehaviour
{
    // Static instance of the class (Singleton)
    private static AvatarSelectionManager _instance;

    // Public static property to access the Singleton instance
    public static AvatarSelectionManager Instance
    {
        get
        {
            // If the instance is not created yet, create one
            if (_instance == null)
            {
                // Create a new GameObject and attach the AvatarSelectionManager component
                _instance = new GameObject("AvatarSelectionManager").AddComponent<AvatarSelectionManager>();
                // Ensure the object persists across scene loads
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    // Properties to store the selected gender and avatar
    public string SelectedGender { get; private set; }
    public string SelectedAvatar { get; private set; }

    // Method to set the selected gender
    public void SetGender(string gender)
    {
        SelectedGender = gender;
        Debug.Log("Selected Gender: " + gender);
    }

    // Method to set the selected avatar
    public void SetAvatar(string avatarName)
    {
        SelectedAvatar = avatarName;
        Debug.Log("Selected Avatar: " + avatarName);
    }

    // Private constructor to prevent instantiation from outside
    private AvatarSelectionManager() { }
}
