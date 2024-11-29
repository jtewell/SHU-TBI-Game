//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class OverlayLoader : MonoBehaviour
//{
//    private static bool isOverlayLoaded = false;

//    private void Awake()
//    {
//        if (!isOverlayLoaded)
//        {
//            //UnityEngine.SceneManagement.SceneManager.LoadScene("SceneName", LoadSceneMode.Single);

//            // Load the overlay scene additively once
//            SceneManager.LoadScene("Overlay", LoadSceneMode.Additive);
//            isOverlayLoaded = true;
//            DontDestroyOnLoad(gameObject);  // Make sure this object persists
//        }

//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;
// IEnumerator 
using System.Collections;




public class SceneElementLoader : MonoBehaviour
{
    public string sceneToLoad = "Overlay"; // Name of the scene to load

    private void Start()
    {
        StartCoroutine(LoadSpecificElements());
    }

    private IEnumerator LoadSpecificElements()
    {
        // Load the scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        yield return asyncLoad;

        // Access the loaded scene
        Scene loadedScene = SceneManager.GetSceneByName(sceneToLoad);

        // Iterate through root objects
        foreach (GameObject rootObject in loadedScene.GetRootGameObjects())
        {
            if (rootObject.name == "UIManager") // Replace with the name of your desired element
            {
                // Move the element to the active scene
                SceneManager.MoveGameObjectToScene(rootObject, SceneManager.GetActiveScene());
            }
            else
            {
                // Deactivate or destroy unwanted elements
                rootObject.SetActive(false);
            }
        }

        // Unload the additive scene to clean up
        SceneManager.UnloadSceneAsync(loadedScene);
    }
}

