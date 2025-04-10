using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMapController : MonoBehaviour
{
    public RectTransform imageRect;
    public GameObject playerMarker;
    public Button compassButton;
    public float movespeed = 20f;
    private float origionalScale = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
         compassButton.onClick.AddListener(MoveToPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        float CurrentScaleFactor = imageRect.localScale.x;
        float scaleFactor = CurrentScaleFactor - origionalScale;
        float speed = movespeed + (scaleFactor * 200f);
        if (Input.GetKey(KeyCode.J))
        {
            imageRect.transform.position -= new Vector3(speed * Time.deltaTime, 0);
            
        }
        if (Input.GetKey(KeyCode.G))
        {
            imageRect.transform.position += new Vector3(speed * Time.deltaTime, 0);
            
        }
        if (Input.GetKey(KeyCode.H))
        {
            imageRect.transform.position += new Vector3(0, speed * Time.deltaTime);
            
        }
        if (Input.GetKey(KeyCode.Y))
        {
            imageRect.transform.position -= new Vector3(0, speed * Time.deltaTime);
        }
            
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log(CurrentScaleFactor);
            Debug.Log(speed);
        }
    }
    private void MoveToPlayer()
    {
        imageRect.transform.position = playerMarker.transform.position;
    }

}
