using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Vector3 GroundDimensions { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeGroundDimensions(string groundPath)
    {
        GameObject ground = GameObject.Find(groundPath);
        if (ground != null)
        {
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
            {
                GroundDimensions = renderer.bounds.size;
            }
            else
            {
                Collider collider = ground.GetComponent<Collider>();
                if (collider != null)
                {
                    GroundDimensions = collider.bounds.size;
                }
            }
        }
    }
}
