using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        // Load the overlay scene additively
        SceneManager.LoadScene("Overlay", LoadSceneMode.Additive);
    }
}
