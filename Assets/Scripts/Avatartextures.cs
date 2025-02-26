using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatartextures: MonoBehaviour
{
    public Material facematerial;
    public Material bodymaterial;
    public Texture[] textureface;
    public Texture[] texturebody;
    public AvatarSciptableObject avi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (avi.Name == "avatar1Container")
        {
            facematerial.mainTexture = textureface[3];
            bodymaterial.mainTexture = texturebody[3];
        }
        else if (avi.Name == "avatar2Container")
        {
            facematerial.mainTexture = textureface[3];
            bodymaterial.mainTexture = texturebody[3];
        }
        else if (avi.Name == "avatar3Container")
        {
            facematerial.mainTexture = textureface[2];
            bodymaterial.mainTexture = texturebody[2];
        }
        else if (avi.Name == "avatar4Container")
        {
            facematerial.mainTexture = textureface[1];
            bodymaterial.mainTexture = texturebody[1];
        }
        else if (avi.Name == "avatar5Container")
        {
            facematerial.mainTexture = textureface[0];
            bodymaterial.mainTexture = texturebody[0];
        }


    }
}
