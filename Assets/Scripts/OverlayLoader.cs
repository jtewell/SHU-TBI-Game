using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayLoader : MonoBehaviour
{
    private static bool isOverlayLoaded = false;

    private void Awake()
    {
        if (!isOverlayLoaded)
        {
            // Load the overlay scene additively once
            SceneManager.LoadScene("Overlay", LoadSceneMode.Additive);
            isOverlayLoaded = true;
            DontDestroyOnLoad(gameObject);  // Make sure this object persists
        }
    }
}
