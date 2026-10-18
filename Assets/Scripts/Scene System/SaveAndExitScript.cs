using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndExitScript : MonoBehaviour
{
    public GameObject sceneManagerObject;
    private SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneManagerObject = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveAndExit()
    {
        SceneManager.LoadScene("ScreeningQuestions");
    }
}
