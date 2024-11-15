using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractableSwitchWorld : MonoBehaviour
{
    public int doorNumber;
    public string Target;
    // Start is called before the first frame update
    public void SwitchScene()
    {

        ScenesManager.Instance.LoadScene(doorNumber, Target);
    }
}
