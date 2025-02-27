using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatartextures: MonoBehaviour
{
    public Material materialFace;
    public Material materialBody;
    public Texture[] textureFace;
    public Texture[] textureBody;
    public AvatarData avi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (avi.Name == "avatar1Container")
        {
            materialFace.mainTexture = textureFace[3];
            materialBody.mainTexture = textureBody[3];
        }
        else if (avi.Name == "avatar2Container")
        {
            materialFace.mainTexture = textureFace[3];
            materialBody.mainTexture = textureBody[3];
        }
        else if (avi.Name == "avatar3Container")
        {
            materialFace.mainTexture = textureFace[2];
            materialBody.mainTexture = textureBody[2];
        }
        else if (avi.Name == "avatar4Container")
        {
            materialFace.mainTexture = textureFace[1];
            materialBody.mainTexture = textureBody[1];
        }
        else if (avi.Name == "avatar5Container")
        {
            materialFace.mainTexture = textureFace[0];
            materialBody.mainTexture = textureBody[0];
        }


    }
}
