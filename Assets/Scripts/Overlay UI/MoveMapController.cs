using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMapController : MonoBehaviour
{
    public RectTransform imageRect;

    public float movespeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.J))
        {
            imageRect.transform.position -= new Vector3(movespeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.G))
        {
            imageRect.transform.position += new Vector3(movespeed * Time.deltaTime, 0);
            
        }
        if (Input.GetKey(KeyCode.H))
        {
            imageRect.transform.position += new Vector3(0, movespeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.Y))
        {
            imageRect.transform.position -= new Vector3(0, movespeed * Time.deltaTime);

        }
    }
}
