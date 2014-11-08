using UnityEngine;
using System.Collections;

[RequireComponent( typeof( SpriteRenderer ) )]
public class AlignSprite: MonoBehaviour
{
    public float spriteWidth
    {
        get
        {
            float width = 1;
            if ( renderer && renderer.sprite != null )
            {
                width = 2f * renderer.sprite.bounds.extents.x;
            }
            return width;
        }
    }

    public float spriteHeight
    {
        get
        {
            float height = 1;
            if ( renderer && renderer.sprite != null )
            {
                height = 2f * renderer.sprite.bounds.extents.y;
            }
            return height;
        }
    }


    public float onScreenX;
    public float onScreenY;

    public bool alignSpriteDims;

    public bool scaleToScreen;
    public bool proportional;
    public float scaleToWidth;
    public float scaleToHeight;


    public Camera cam;


    new SpriteRenderer renderer;




    public void SetScreenPos( float x, float y )
    {
        onScreenX = x;
        onScreenY = y;
    }

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        align();
    }

    void Update()
    {
        align();
    }

    public void align()
    {
        if ( !cam )
        {
            cam = Camera.main;
        }

        Vector3 newPosition = transform.position;

        Vector3 screenNormalized = Vector3.right * ( ( onScreenX + 1f ) / 2 ) * Screen.width
                                    + Vector3.up * ( ( onScreenY + 1f ) / 2 ) * Screen.height;


        Vector3 pointFromScreen = cam.ScreenToWorldPoint( screenNormalized );
        newPosition.x = pointFromScreen.x;
        newPosition.y = pointFromScreen.y;

        if ( alignSpriteDims )
        {
            if ( onScreenX > 0 )
            {
                newPosition.x -= spriteWidth;
            }

            if ( onScreenY < 0 )
            {
                newPosition.y += spriteHeight;
            }
        }


        Vector3 newScale = Vector3.one;

        if ( scaleToScreen )
        {
            if ( scaleToWidth > 0 )
            {
                newScale.x = scaleToWidth * 2 * cam.orthographicSize * cam.aspect / spriteWidth;
                newPosition.x -= 0.5f * newScale.x * spriteWidth;
            }

            if ( scaleToHeight > 0 )
            {
                newScale.y = scaleToHeight * 2 * cam.orthographicSize / spriteHeight;
                newPosition.y += 0.5f * newScale.y * spriteHeight;
            }

            if ( proportional )
            {
                newScale = Vector3.one * Mathf.Max( newScale.x, newScale.y );
            }

            //newPosition.x -= 0.5f * newScale.x * spriteWidth;
            //newPosition.y += 0.5f * newScale.y * spriteHeight;
            transform.localScale = newScale;
        }

        transform.position = newPosition;
    }
}
