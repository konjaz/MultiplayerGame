using UnityEngine;
using System.Collections;

public class Frame: MonoBehaviour
{
    public Camera mainCam;

    public float viewportWidth = 0.7f;

    public AlignSprite leftTop;
    public AlignSprite rightTop;
    public AlignSprite leftDown;
    public AlignSprite rightDown;

    void Start()
    {
        if ( !mainCam )
        {
            mainCam = GameObject.FindGameObjectWithTag( "MainCamera" )
                .GetComponent<Camera>();
        }

        align();
    }

    void Update()
    {
        align();
    }

    public void align()
    {
        // assume sprites are squares 
        float spriteAspectWidth = (float)32 / Screen.width;
        float spriteAspectHeight = (float)32 / Screen.height;


        //Debug.Log( spriteAspectWidth + " " + spriteAspectHeight );

        mainCam.rect = new Rect( 0, 0, viewportWidth, 1f );
    }
}
