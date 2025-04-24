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
    private float boarderX = 30f;
    private float boarderY =  20f;
    private float mapOffsety = 266.25f;
    private float mapOffsetx = 436.25f;
    
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
        float scaleBorderX=(scaleFactor*10) * boarderX;
        float scaleBorderY= (scaleFactor*10) * boarderY;
        float speed = movespeed + (scaleFactor * 200f);
        if (Input.GetKey(KeyCode.J))
        {
            if (imageRect.transform.position.x - mapOffsetx <= -scaleBorderX)
            {

                imageRect.transform.position -= new Vector3(0, 0);
            }
            else
            {

                imageRect.transform.position -= new Vector3(speed * Time.deltaTime, 0);

            }
        }
        if (Input.GetKey(KeyCode.G))
        {
            if (imageRect.transform.position.x - mapOffsetx >= scaleBorderX)
            {

                imageRect.transform.position += new Vector3(0, 0);
            }
            else
            {

                imageRect.transform.position += new Vector3(speed * Time.deltaTime, 0);

            }
        }
        if (Input.GetKey(KeyCode.H))
        {
            if (imageRect.transform.position.y - mapOffsety >= scaleBorderY)
            {

                imageRect.transform.position += new Vector3(0, 0);
            }
            else
            {

                imageRect.transform.position += new Vector3(0, speed * Time.deltaTime);

            }
        }
        if (Input.GetKey(KeyCode.Y))
        {
            if (imageRect.transform.position.y - mapOffsety <= -scaleBorderY)
            {
                
                imageRect.transform.position -= new Vector3(0, 0);
            }
            else
            {

                imageRect.transform.position -= new Vector3(0, speed * Time.deltaTime);

            }
        }
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log(imageRect.transform.position.x);
            Debug.Log(scaleBorderX);
            Debug.Log(speed);
        }
    }
    private void MoveToPlayer()
    {
        imageRect.transform.position = playerMarker.transform.position;
    }

}
