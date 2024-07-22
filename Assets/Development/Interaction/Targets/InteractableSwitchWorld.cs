using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractableSwitchWorld : MonoBehaviour
{
    public string Target;
    // Start is called before the first frame update
    public void SwitchScene()
    {
        SceneManager.LoadScene(Target);
    }
}
