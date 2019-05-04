using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    /*
     * Lerps a given transform between two other transforms at a given speed. Waits for either the end of the frame or fixed update depending on the final flag
     */
    public static IEnumerator LerpBetweenTransformsOverTime(Transform transformToMove, Transform pos1, Transform pos2, float speed, bool waitForEndFrame)
    {
        float StartTime = Time.time;
        float journeyLength = Vector3.Distance(pos1.position, pos2.position);
        float fractionJourneyCovered = 0;

        //keep going until the entire journey has been covered
        while(fractionJourneyCovered < 1)
        {
            float distCovered = (Time.time - StartTime) * speed;
            fractionJourneyCovered = distCovered / journeyLength;

            transformToMove.position = Vector3.Lerp(pos1.position, pos2.position, fractionJourneyCovered);

            if(waitForEndFrame)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    /*
     * Takes in a BoxCollider2D and a sprite and sets the box collider's size to the bounds of the sprite
     */
    public static void ResetBoxCollider2DBoundsToSpriteBounds(BoxCollider2D collider, Sprite sprite)
    {
        Vector2 spriteBounds = sprite.bounds.size;

        collider.size = spriteBounds;
    }
}
